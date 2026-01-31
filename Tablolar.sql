
-- 1. Müşteri Tablosu
CREATE TABLE musteri (
    musteri_id SERIAL PRIMARY KEY,
    isim VARCHAR(100),
    soyisim VARCHAR(100),
    eposta VARCHAR(150),
    telefon VARCHAR(20),
    bakiye NUMERIC(10,2) DEFAULT 0
);

-- 2. Portföy Tablosu
CREATE TABLE portfoy (
    portfoy_id SERIAL PRIMARY KEY,
    musteri_id INT REFERENCES musteri(musteri_id),
    varlik_degeri NUMERIC(15,2)
);

-- 3. Satıcı Tablosu
CREATE TABLE satici (
    satici_id SERIAL PRIMARY KEY,
    musteri_id INT REFERENCES musteri(musteri_id),
    satici_adi VARCHAR(150),
    baglanti VARCHAR(255)
);

-- 4. Ürün Tablosu
CREATE TABLE urun (
    urun_id SERIAL PRIMARY KEY,
    urun_adi VARCHAR(200),
    kategori VARCHAR(100),
    fiyat NUMERIC(10,2),
    satici_id INT REFERENCES satici(satici_id),
    stok_adedi INT DEFAULT 0 
);

ALTER TABLE urun
ADD CONSTRAINT stok_negatif_olamaz
CHECK (stok_adedi>=0)


-- 5. Stok Tablosu
CREATE TABLE stok (
    stok_id SERIAL PRIMARY KEY,
    urun_id INT REFERENCES urun(urun_id),
    miktar INT DEFAULT 0
);

-- 6. Sipariş Tablosu
CREATE TABLE siparis (
    siparis_id SERIAL PRIMARY KEY,
    musteri_id INT REFERENCES musteri(musteri_id),
    tarih TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(),
    toplam_tutar NUMERIC(10,2)
);

-- 7. Sipariş Detay Tablosu
CREATE TABLE siparis_detay (
    siparis_id INT REFERENCES siparis(siparis_id),
    urun_id INT REFERENCES urun(urun_id),
    miktar INT,
    satis_fiyati NUMERIC(10,2),
    PRIMARY KEY (siparis_id, urun_id)
);

-- 8. İşlem Geçmişi
CREATE TABLE islem_gecmisi (
    gecmis_id SERIAL PRIMARY KEY,
    musteri_id INT REFERENCES musteri(musteri_id),
    siparis_id INT REFERENCES siparis(siparis_id),
    tarih TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW()
);

-- 9. Piyasa Analizi
CREATE TABLE piyasa_analizi (
    analiz_id SERIAL PRIMARY KEY,
    tarih DATE DEFAULT CURRENT_DATE,
    ekonomik_durum VARCHAR(100),
    fiyat_onerisi NUMERIC(10,2),
    notlar TEXT
);

-- 10. Fiyatlandırma Tablosu
CREATE TABLE fiyatlandirma (
    fiyatlandirma_id SERIAL PRIMARY KEY,
    urun_id INT REFERENCES urun(urun_id),
    piyasa_analiz_id INT REFERENCES piyasa_analizi(analiz_id),
    belirlenen_fiyat NUMERIC(10,2) 
);

-- 11. Kampanya Tablosu
CREATE TABLE kampanya (
    kampanya_id SERIAL PRIMARY KEY,
    baslangic_tarihi TIMESTAMP WITHOUT TIME ZONE,
    bitis_tarihi TIMESTAMP WITHOUT TIME ZONE,
    indirim_orani NUMERIC(5,2)
);

-- 12. Ürün Kampanya (Composite Key)
CREATE TABLE urun_kampanya (
    urun_id INT REFERENCES urun(urun_id),
    kampanya_id INT REFERENCES kampanya(kampanya_id),
    PRIMARY KEY (urun_id, kampanya_id)
);

