using System.Drawing.Text;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace zeynepsarioglu231038006toyun
{
    public partial class Form1 : Form
    {
        // Gerekli değişkenler 
        int hedef_sayi;
        int tahmin_sayisi = 0;
        int puan = 500;
        private string log;
        private bool Gecerli_tahmin;
        private int dogru_yer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Enabled = false; // buton1, 'tahmin' butonudur. Oyun başlamadan tahmin yapamayız. Bu yüzden ilk aşamada kapattık.
            button2.Enabled = true;  // Oyunu başlatmak için bu aktif olmalı.
            textBox1.Enabled = false; // Oyun başladıktan sonra içerisine yazı yazabilmemiz için aktif olacak.
            textBox1.MaxLength = 4; // En uzun 4 basamaklı bir sayı yazabiliriz.
            label6.Text = "0";  // İçerisi boş olacak. Tahmin sayısına göre değişecek.
            label7.Text = "500";  // Başlangıçta 500 puan gösterilecek. Yanlış cevaba göre azalacak.
            label4.Text = "Puan";  // Puan gösterilecek.


        }
        private int sayi_uret() // Bilgisayar bizim tahmin etmemiz için bir sayı tutmalı.
        {
            Random rnd = new Random();
            int sayi;

            do
            {
                sayi = rnd.Next(1000, 2000); // 4 basamaklı sayı üret(1000-2000 arasında).
            } while (sayi.ToString().Distinct().Count() != 4 || sayi.ToString()[0] == '0');

            return sayi;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            hedef_sayi = sayi_uret();
            tahmin_sayisi = 0;
            puan = 500;
            label7.Text = "500";  // Başlangıçta 500.
            label4.Text = $"Puan";  // label4'te puan göster.
            label6.Text = "0";
            listBox1.Items.Clear();
            textBox1.Clear();

            button1.Enabled = true; // 2. butona tıkladıktan sonra oyun başlıyor ve buton 1 yani 'tahmin' butonu aktif oluyor.
            button2.Enabled = false;
            textBox1.Enabled = true; // Artık içerisine tahmini sayımızı yazabiliriz.
            MessageBox.Show("Yeni oyun başladı. 4 basamaklı bir sayı tuttum."); // Ekranda gösterilecek.
            textBox1.Focus();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tahmin = textBox1.Text.Trim();

            int kontrol;
            if (tahmin.Length != 4 || tahmin.Distinct().Count() != 4 || tahmin[0] == '0' || !int.TryParse(tahmin, out kontrol))
            {
                MessageBox.Show("Lütfen 0 ile başlamayan, rakamları birbirinden farklı 4 basamaklı bir sayı giriniz.");
                return;
            }
            // Girilen sayı 4 basamaklı mı? Girilen sayıdaki rakamlar birbirinden farklı mı? Sayı 0 ile mi başlıyor? Girilen değer sayısal mı?
            // Yukarıdaki yazdığımız kod, buton1'e tıklandıktan sonra bilgisayarın belirtilen soruların sırasıyla yaptığı kontrollerdir.
            // Eğer gerekli şartlar sağlanmazsa messagebox gösterilir.

            tahmin_sayisi++;
            label6.Text = tahmin_sayisi.ToString(); // Her tahminden sonra tahmin sayısı label6' da gösterilecek şekilde artıyor.

            string hedefStr = hedef_sayi.ToString(); // Karakter-karakter karşılaştırma için.

            int dogru_yer = 0;
            int yanlis_yer = 0;

            for (int i = 0; i < 4; i++)
            {
                if (tahmin[i] == hedefStr[i])
                    dogru_yer++;
                else if (hedefStr.Contains(tahmin[i]))
                    yanlis_yer++;
            }

            // Eğer tüm 4 rakam da doğru yerdeyse, oyuncu oyunu kazanmıştır.
            //Tebrik mesajı gösterilir, deneme sayısı ve puan da bildirilir.
            // Eğer 5 tahmin yapılmışsa ve doğru sayı bulunamamışsa oyun biter.

            if (dogru_yer == 4)
            {
                MessageBox.Show($"Tebrikler! {tahmin_sayisi}. denemede doğru bildiniz.\nPuanınız: {puan}");
                OyunuSifirla();
                return;
            }

            if (tahmin_sayisi == 5)
            {
                MessageBox.Show($"Tahmin hakkınız tükendi. Doğru sayı: {hedef_sayi}");
                OyunuSifirla();
                return;
            }

            // Yanlış tahminde puanı, 50 puan azalt.
            puan -= 50;
            label7.Text = $"Puan: {puan}"; // Puanı label7'de güncelle.

            listBox1.Items.Add($"{tahmin} → {dogru_yer} doğru yerde, {yanlis_yer} yanlış yerde"); // Her tahminde listbox'ta bilgilendirme yapılıyor.

            textBox1.Clear();
            textBox1.Focus();

        }
        // Tahmin olarak girilen sayının özelliklerini kontrol ediyor.
        private bool GecerliTahmin(string tahmin)
        {
            if (tahmin.Length != 4)
                return false;
            if (tahmin.Distinct().Count() != 4)
                return false;
            if (!tahmin.All(char.IsDigit))
                return false;
            if (tahmin[0] == '0')
                return false;

            return true;
        }

        // Tahmin hakkı bittiğinde veya doğru sayı bilindiğinde oyun bitiyor. 'Tahmin' buton inaktif, 'Başla' butonu aktif ediliyor.
        private void OyunuSifirla()
        {
            button1.Enabled = false;
            button2.Enabled = true;
            textBox1.Enabled = false;
        }

    }
}

