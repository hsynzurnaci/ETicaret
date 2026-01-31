-- KISITLAMA (CONSTRAINT)
ALTER TABLE urun
ADD CONSTRAINT stok_negatif_olamaz
CHECK (stok_adedi>=0)


-- 1. VERİ EKLEME (INSERT)
TRUNCATE TABLE siparis_detay, siparis, stok, urun, satici, musteri, piyasa_analizi, fiyatlandirma RESTART IDENTITY CASCADE;

-- Müşteriler
INSERT INTO musteri (isim, soyisim, eposta, telefon, bakiye) VALUES 
('Ahmet', 'Yılmaz', 'ahmet@mail.com', '5551112233', 15000),
('Ayşe', 'Demir', 'ayse@mail.com', '5554445566', 25000),
('Mehmet', 'Kaya', 'mehmet@mail.com', '5557778899', 5000);

-- Satıcı (Ahmet aynı zamanda satıcı olsun)
INSERT INTO satici (musteri_id, satici_adi, baglanti) VALUES 
(1, 'TeknoStore', 'www.teknostore.com');

-- Ürünler
INSERT INTO urun (urun_adi, kategori, fiyat, satici_id) VALUES 
('Gaming Laptop', 'Elektronik', 35000, 1),
('Kablosuz Mouse', 'Elektronik', 750, 1),
('Mekanik Klavye', 'Elektronik', 1500, 1),
('Çalışma Masası', 'Mobilya', 4000, 1);

-- Stok (1-1 İlişki)
INSERT INTO stok (urun_id, miktar) VALUES 
(1, 5),  -- Laptop Stoğu
(2, 50), -- Mouse Stoğu
(3, 20), -- Klavye Stoğu
(4, 10); -- Masa Stoğu

-- Piyasa Analizi
INSERT INTO piyasa_analizi (ekonomik_durum, fiyat_onerisi) VALUES 
('Enflasyon Yüksek', 38000);

-- Fiyatlandırma (Ürün ile Analiz İlişkisi)
INSERT INTO fiyatlandirma (urun_id, piyasa_analiz_id, belirlenen_fiyat) VALUES 
(1, 1, 38000); -- Laptop'un analize göre olması gereken fiyatı


-- ---------------------------------------------------------


-- 2. TETİKLEYİCİ (TRIGGER)

-- Sipariş verildiğinde, islem_gecmisi tablosuna log'layan Tetikleyici.

CREATE OR REPLACE FUNCTION fn_siparis_logla()
RETURNS TRIGGER AS $$
BEGIN
    INSERT INTO islem_gecmisi (musteri_id, siparis_id, tarih)
    VALUES (NEW.musteri_id, NEW.siparis_id, NOW());
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_siparis_eklendi ON siparis;
CREATE TRIGGER trg_siparis_eklendi
AFTER INSERT ON siparis
FOR EACH ROW
EXECUTE FUNCTION fn_siparis_logla();

-- Stok Düşürme Tetikleyicisi. -------------------------------------

CREATE OR REPLACE FUNCTION fn_stok_dus()
RETURNS TRIGGER AS $$
BEGIN
    UPDATE urun
    SET stok_adedi = stok_adedi - NEW.miktar
    WHERE urun_id = NEW.urun_id;
   
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_satis_sonrasi_stok_dus ON siparis_detay;

CREATE TRIGGER trg_satis_sonrasi_stok_dus
AFTER INSERT ON siparis_detay
FOR EACH ROW
EXECUTE FUNCTION fn_stok_dus();


-- ---------------------------------------------------------


-- 3. SAKLI PROSEDÜR (STORED PROCEDURE)
-- Kategoriye Özel İndirim 

CREATE OR REPLACE PROCEDURE KategoriIndirimiYap(
    p_KategoriAdi VARCHAR,  
    p_IndirimOrani DECIMAL  
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF p_IndirimOrani <= 0 THEN
        RAISE EXCEPTION 'İndirim oranı 0 veya negatif olamaz!';
    END IF;
    UPDATE urun
    SET Fiyat = Fiyat - (Fiyat * (p_IndirimOrani / 100))
    WHERE Kategori = p_KategoriAdi;
    RAISE NOTICE '% kategorisine %% % oranında indirim uygulandı.', p_KategoriAdi, p_IndirimOrani;
END;
$$;


-- ---------------------------------------------------------


-- 4. GÖRÜNÜM (VIEW)
-- Ürün Stok Raporu
CREATE OR REPLACE VIEW vw_urun_stok_raporu AS
SELECT 
    u.urun_id,
    u.urun_adi,
    u.kategori,
    u.fiyat,
    u.stok_adedi,  
    sat.satici_adi,
    (u.fiyat * COALESCE(u.stok_adedi, 0)) AS toplam_stok_degeri -- Stok boşsa 0 say
FROM urun u
LEFT JOIN satici sat ON u.satici_id = sat.satici_id;

-- ---------------------------------------------------------


-- 5. AGGREGATE FUNCTION (FONKSİYON)
-- Kategorilerin Ortalama Fiyatı

CREATE OR REPLACE FUNCTION fn_kategori_ortalama_fiyat(p_kategori VARCHAR)
RETURNS NUMERIC AS $$
DECLARE
    ortalama NUMERIC;
BEGIN
    SELECT AVG(fiyat) INTO ortalama
    FROM urun
    WHERE kategori = p_kategori;
    
    RETURN COALESCE(ortalama, 0);
END;
$$ LANGUAGE plpgsql;


-- ---------------------------------------------------------


-- 6. KURSOR (CURSOR)
--piyasa_analizi 'nden aldığı verilerle, düşük fiyatlı ürünlere zam yapar.

CREATE OR REPLACE PROCEDURE sp_fiyatlari_guncelle_cursor()
LANGUAGE plpgsql
AS $$
DECLARE
    cur_fiyatlar CURSOR FOR 
        SELECT u.urun_id, p.fiyat_onerisi 
        FROM urun u
        JOIN fiyatlandirma f ON u.urun_id = f.urun_id
        JOIN piyasa_analizi p ON f.piyasa_analiz_id = p.analiz_id
        WHERE u.fiyat < p.fiyat_onerisi;
        
    v_urun_id INT;
    v_yeni_fiyat NUMERIC;
BEGIN
    OPEN cur_fiyatlar;
    
    LOOP
        FETCH cur_fiyatlar INTO v_urun_id, v_yeni_fiyat;
        EXIT WHEN NOT FOUND;
        
  
        UPDATE urun SET fiyat = v_yeni_fiyat WHERE urun_id = v_urun_id;
    END LOOP;
    
    CLOSE cur_fiyatlar;
END;
$$;

-- 7. PARAMETRELİ EKLE
-- Ürün Ekleme

CREATE OR REPLACE PROCEDURE UrunEkle(
    p_UrunAdi VARCHAR,
    p_Kategori VARCHAR,
    p_Fiyat DECIMAL,
    p_SaticiId INT,
	p_StokAdedi INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO urun (urun_adi, kategori, fiyat, satici_id, stok_adedi)
    VALUES (p_UrunAdi, p_Kategori, p_Fiyat, p_SaticiId, p_StokAdedi);
END;
$$;




