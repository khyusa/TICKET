using System.Collections;
using System.Reflection.Emit;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;

namespace ticket
{
    public partial class Form1 : Form
    {
        bool customerFilter = false;
        bool mission = true;
        int tries = 4;
        int zaman = 180;
        public static ArrayList ticket = new ArrayList();
        public static ArrayList beklemede = new ArrayList();
        public static ArrayList onemli = new ArrayList();
        public static ArrayList acil = new ArrayList();
        public static ArrayList customers = new ArrayList();
        public static ArrayList latestCustomer = new ArrayList();
        public static ArrayList deletedCustomers = new ArrayList();
        public readonly char[] bannedchars = "\"\'é!^+%&/()=?-*}][{$#@~,;`.:|´".ToCharArray();


        public Form1()
        {
            InitializeComponent();
        }

        public void bilgileriGuncelle()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ticket Panel Info");
            string filePath = Path.Combine(folderPath, "User.txt");
            string passPath = Path.Combine(folderPath, "Password.txt");

            // Klasör yoksa oluþtur
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Directory.CreateDirectory(folderPath);
            }

            // TextBox boþsa kaydetme
            if (string.IsNullOrWhiteSpace(textBox11.Text) || string.IsNullOrWhiteSpace(textBox12.Text))
            {
                MessageBox.Show("Boþ veri kaydedilemez!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TXT dosyasýna veriyi yaz
            hesaplabel.Text = textBox11.Text.Trim();
            sifrelabel.Text = textBox12.Text.Trim();
            this.Text = "TICKET Yönetme & Ýnceleme Paneli (" + hesaplabel.Text.Trim() + ")";
            File.WriteAllText(filePath, textBox11.Text.Trim());
            File.WriteAllText(passPath, textBox12.Text.Trim());
        }
        public void musteriArrayList()
        {
            if (customers.Count == 0)
            {
                if (!checkBox15.Checked)
                {
                    MessageBox.Show("ArrayList boþ gibi gözüküyor..", "ArrayList Boþ", MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
                listBox9.Items.Clear();
                listBox9.Items.Add("Liste Boþ...");
                return;
            }
            else
            {
                if (checkBox16.Checked)
                {
                    if (numericUpDown1.Value > latestCustomer.Count || numericUpDown1.Value <= 0)
                    {
                        MessageBox.Show("Geçersiz miktar veya ArrayList boþ..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        numericUpDown1.Value = customers.Count;
                        return;
                    }
                    else
                    {
                        int sayac = 0;
                        listBox9.Items.Clear();
                        foreach (string custom in latestCustomer)
                        {
                            if (numericUpDown1.Value != 0)
                            {
                                numericUpDown1.Value--;
                                if (!checkBox14.Checked)
                                {
                                    listBox9.Items.Add(custom);
                                }
                                else
                                {
                                    sayac++;
                                    listBox9.Items.Add(sayac.ToString() + "-) " + custom);
                                }
                            }
                            else
                            {
                                numericUpDown1.Value = listBox9.Items.Count;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (!checkBox14.Checked)
                    {
                        listBox9.Items.Clear();
                        foreach (string items in latestCustomer)
                        {
                            listBox9.Items.Add(items);
                        }
                    }
                    else
                    {
                        int sayac = 0;
                        listBox9.Items.Clear();
                        foreach (string items in latestCustomer)
                        {
                            sayac++;
                            listBox9.Items.Add(sayac.ToString() + "-) " + items);
                        }
                    }
                }
            }
        }
        public void musteriOlustur()
        {
            try
            {
                if (progressBar3.Value == progressBar3.Maximum)
                {
                    MessageBox.Show("Sýnýrlý müþteri sayýsýna ulaþtýnýz, birazýný silmeyi deneyin..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    tabControl2.Enabled = false;
                    if (!string.IsNullOrWhiteSpace(textBox2.Text.Trim()) && !customers.Contains(textBox2.Text.Trim().ToUpper()))
                    {
                        listBox3.Items.Add(textBox2.Text.Trim().ToUpper());
                        comboBox2.Items.Add(textBox2.Text.Trim().ToUpper());
                        customers.Add(textBox2.Text.Trim().ToUpper());
                        latestCustomer.Add(textBox2.Text.Trim().ToUpper());
                        textBox2.Clear();
                        if (!checkBox10.Checked)
                        {
                            MessageBox.Show("Müþteri baþarýyla eklendi.", "Müþteri Eklendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        tabControl2.Enabled = true;
                        if (checkBox12.Checked)
                        {
                            listBox2.Items.Clear();
                            int madde = 0;
                            foreach (string customer in customers)
                            {
                                madde++;
                                listBox2.Items.Add(madde.ToString() + "-)");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("\"" + textBox2.Text.Trim().ToUpper() + "\"" + " isimli müþteri zaten ekli.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        textBox2.Clear();
                        tabControl2.Enabled = true;
                    }
                }
            }
            catch
            {
                textBox2.Clear();
                MessageBox.Show("Birþeyler ters gitti. Lütfen daha sonra tekrar deneyiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            comboBox3.Items.Clear();
            foreach (string items in customers)
            {
                comboBox3.Items.Add(items);
            }
        }
        private void bilgileriGir()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ticket Panel Info");
            string filePath = Path.Combine(folderPath, "User.txt");
            string passPath = Path.Combine(folderPath, "Password.txt");

            if (Directory.Exists(folderPath)) // Eðer klasör varsa
            {
                if (File.Exists(filePath)) // Eðer dosya varsa
                {
                    string[] lines = File.ReadAllLines(filePath);
                    hesaplabel.Text = lines[0].ToString(); // Ýlk satýrý label'a yaz
                    this.Text = "TICKET Yönetme & Ýnceleme Paneli (" + hesaplabel.Text.Trim() + ")";
                }
                if (File.Exists(passPath))
                {
                    string[] lines = File.ReadAllLines(passPath);
                    sifrelabel.Text = lines[0].ToString(); // Ýlk satýrý label'a yaz
                }
            }
            else
            {
                this.Text = "TICKET Yönetme & Ýnceleme Paneli";
                Directory.CreateDirectory(folderPath);
                File.WriteAllText(filePath, " ");
                File.WriteAllText(passPath, " ");
                return;
            }
        }
        private void panelOlustur()
        {
            ticket.Clear();
            customers.Clear();
            listBox1.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            listBox6.Items.Clear();
            listBox7.Items.Clear();
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ticket Panel Info");
            string filePath = Path.Combine(folderPath, "User.txt");
            string passPath = Path.Combine(folderPath, "Password.txt");

            // Klasör yoksa oluþtur
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else
            {
                Directory.CreateDirectory(folderPath);
            }

            // TextBox boþsa kaydetme
            if (string.IsNullOrWhiteSpace(textBox11.Text) || string.IsNullOrWhiteSpace(textBox12.Text))
            {
                MessageBox.Show("Boþ veri kaydedilemez!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // TXT dosyasýna veriyi yaz
            hesaplabel.Text = textBox11.Text.Trim();
            sifrelabel.Text = textBox12.Text.Trim();
            this.Text = "TICKET Yönetme & Ýnceleme Paneli (" + hesaplabel.Text.Trim() + ")";
            File.WriteAllText(filePath, textBox11.Text.Trim());
            File.WriteAllText(passPath, textBox12.Text.Trim());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (mission == true)
            {
                label2.Text = textBox1.Text;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker1.Text = DateTime.Now.ToString();
            }
            catch
            {
                MessageBox.Show("Bu alandaki tarihi deðiþtiremezsiniz, eðer gelecek planýnýz varsa alttaki \"Planlý TICKET\" seçeneðini iþaretleyebilirsiniz.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bilgileriGir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox1.Text) || String.IsNullOrWhiteSpace(comboBox1.Text) || String.IsNullOrWhiteSpace(comboBox2.Text) || String.IsNullOrWhiteSpace(textBox5.Text))
            {
                comboBox1.ResetText();
                comboBox2.ResetText();
                MessageBox.Show("Hiçbir kutu boþ girilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox5.TextLength > 100)
            {
                MessageBox.Show("Açýklama fazla uzun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                button1.Text = "TICKET oluþturuluyor, lütfen ayrýlmayýnýz..";
                panel2.Enabled = false;
                progressBar1.Value = 0;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = false;
                panel2.Enabled = false;
                progressBar1.Visible = true;
                progressBar1.Value++;
            }
            catch
            {
                progressBar1.Value = progressBar1.Maximum;
                timer1.Stop();
                panel1.Enabled = true;
                panel2.Enabled = true;
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                if (textBox1.TextLength < 3 || textBox1.TextLength > 30 || textBox5.TextLength > 600)
                {
                    MessageBox.Show("Bir hata oluþtu, lütfen daha sonra tekrar deneyiniz..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button1.Text = "TICKET oluþtur";
                    button1.Enabled = true;
                    return;
                }
                else
                {
                    if (radioButton1.Checked == true)
                    {
                        MessageBox.Show("TICKET'ýnýz baþarýyla oluþturuldu.", "TICKET baþarýyla oluþturuldu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        saveTICKET();
                        button1.Text = "TICKET oluþtur";
                        textBox1.Clear();
                        comboBox2.Text = "";
                        textBox3.Clear();
                        textBox5.Clear();
                        label7.Text = "";
                        comboBox1.Text = "";
                        dateTimePicker1.Value = DateTime.Now;
                        label1.Text = "";
                        checkBox1.Checked = false;
                        checkBox9.Checked = false;
                        dateTimePicker3.Enabled = false;
                        dateTimePicker3.ResetText();
                    }
                    else
                    {
                        saveTICKET();
                        button1.Text = "TICKET oluþtur";
                        textBox1.Clear();
                        comboBox2.Text = "";
                        textBox3.Clear();
                        textBox5.Clear();
                        label7.Text = "";
                        comboBox1.Text = "";
                        dateTimePicker1.Value = DateTime.Now;
                        label1.Text = "";
                        checkBox1.Checked = false;
                        checkBox9.Checked = false;
                        dateTimePicker3.Enabled = false;
                        dateTimePicker3.ResetText();
                    }
                }
                foreach (Control radio in groupBox21.Controls)
                {
                    if (radio is RadioButton radioButton)
                    {
                        radioButton.Checked = false;
                    }
                }
                radioButton3.Checked = true;
            }
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                mission = false;
                textBox1.Enabled = false;
                textBox1.Text = "Adsýz Kiþi";
            }
            else
            {
                mission = true;
                textBox1.Enabled = true;
                textBox1.Text = label2.Text;
            }
        }
        private void saveTICKET()
        {
            try
            {
                if (checkBox9.Checked == true)
                {
                    string ticketText = "Müþteri: " + comboBox2.Text.Trim() + " (" + textBox1.Text.Trim() + ")" + "\nKonu: " + comboBox1.Text.Trim() + "\nAçýklama: \n" + textBox5.Text.Trim();
                    ticketText += "\nTICKET Oluþturulma Tarihi: " + dateTimePicker1.Text + "\nTICKET Plan Tarihi: " + dateTimePicker3.Text;
                    ticket.Add(ticketText);
                    if (radioButton3.Checked)
                    {
                        beklemede.Add(ticketText + "\n(Bekleyebilir)");
                    }
                    else if (radioButton4.Checked)
                    {
                        onemli.Add(ticketText + "\n(Önemli)");
                    }
                    else if (radioButton5.Checked)
                    {
                        acil.Add(ticketText + "\n(Acil)");
                    }
                }
                else
                {
                    string ticketText = "Müþteri: " + comboBox2.Text.Trim() + " (" + textBox1.Text.Trim() + ")" + "\nKonu: " + comboBox1.Text.Trim() + "\nAçýklama: \n" + textBox5.Text.Trim();
                    ticketText += "\nTICKET Oluþturulma Tarihi: " + dateTimePicker1.Text;
                    ticket.Add(ticketText);
                    if (radioButton3.Checked)
                    {
                        beklemede.Add(ticketText + "\n(Bekleyebilir)");
                    }
                    else if (radioButton4.Checked)
                    {
                        onemli.Add(ticketText + "\n(Önemli)");
                    }
                    else if (radioButton5.Checked)
                    {
                        acil.Add(ticketText + "\n(Acil)");
                    }
                }
                listBox1.Items.Add(comboBox2.Text.Trim() + " (" + textBox1.Text.Trim() + ") Dosyasý");
                listBox4.Items.Add(comboBox2.Text.Trim() + " (" + textBox1.Text.Trim() + ")");
                listBox6.Items.Add(comboBox1.Text.Trim());
                listBox7.Items.Add("Oluþturuldu");
            }
            catch
            {
                MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox3.PasswordChar = textBox1.PasswordChar;
            }
            else
            {
                textBox3.PasswordChar = '*';
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textBox3.Text) || String.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Boþ deðer girilemez.", "Giriþ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (tries <= 1)
                {
                    tries = 4;
                    textBox3.Clear();
                    textBox4.Clear();
                    checkBox1.Checked = false;
                    MessageBox.Show("Tüm giriþ haklarýnýzý kullandýnýz. Geri gönderiliyorsunuz.", "Giriþ (baþarýsýz)", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                }
                else
                {
                    if (textBox4.Text == hesaplabel.Text && textBox3.Text == sifrelabel.Text)
                    {
                        tries = 4;
                        MessageBox.Show("Giriþ baþarýlý. Hoþgeldiniz, @" + hesaplabel.Text + "!", "Giriþ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        panel1.Visible = false;
                        tabControl1.Visible = true;
                        textBox3.Clear();
                        textBox4.Clear();
                        checkBox2.Checked = false;
                    }
                    else
                    {
                        if (textBox4.Text.Length < 3 || textBox4.Text.Length > 20)
                        {
                            textBox4.Clear();
                            label4.Visible = true;
                        }
                        else if (textBox3.Text.Length < 8 || textBox3.TextLength > 20)
                        {
                            textBox3.Clear();
                            label5.Visible = true;
                        }
                        else
                        {
                            if (String.IsNullOrWhiteSpace(textBox3.Text) || String.IsNullOrWhiteSpace(textBox4.Text))
                            {
                                MessageBox.Show("Boþ deðer girilemez.", "Giriþ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                label4.Visible = false;
                                label5.Visible = false;
                            }
                            else
                            {
                                tries--;
                                textBox3.Clear();
                                textBox4.Clear();
                                MessageBox.Show("Hatalý deðer veya deðerler girdiniz. Kalan deneme hakkýnýz: " + tries.ToString(), "Giriþ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                label4.Visible = false;
                                label5.Visible = false;
                            }
                        }
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Focus();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            comboBox1.ResetText();
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            comboBox1.Text = "";
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label7.Text = (100 - textBox5.TextLength).ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (zaman <= 0)
            {
                timer2.Stop();
                zaman = 0;
                label9.Text = "";
                MessageBox.Show("Doðrulama için verilen süreniz doldu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                zaman = 180;
                textBox10.Clear();
                textBox10.Enabled = true;
                linkLabel3.Text = "Doðrula";
            }
            else
            {
                zaman--;
                label9.Text = zaman.ToString();
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.TextLength < 3 || textBox2.TextLength > 15)
            {
                MessageBox.Show("Geçersiz veri. (Yazý kutucuðu boþ olamaz, veya girilen veri 3-25 arasý karakter içermelidir)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (checkBox8.Checked)
                {
                    MessageBox.Show(textBox2.Text.Trim().ToUpper() + Environment.NewLine + "Oluþturma Tarih: " + dateTimePicker2.Text, "Müþteri Önizleme");
                }
                else
                {
                    MessageBox.Show(textBox2.Text.Trim().ToUpper(), "Müþteri Önizleme");
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.TextLength < 3 || textBox2.TextLength > 25)
            {
                MessageBox.Show("Geçersiz veri. (Yazý kutucuðu boþ olamaz, veya girilen veri 3-25 arasý karakter içermelidir)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!checkBox11.Checked)
                {
                    DialogResult lastStep = MessageBox.Show("Müþteri: " + textBox2.Text.Trim().ToUpper() + Environment.NewLine + "Oluþturma Tarihi: " + dateTimePicker2.Text, "Müþteri Oluþtur", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    switch (lastStep)
                    {
                        case DialogResult.Cancel:
                            break;
                        case DialogResult.OK:
                            musteriOlustur();
                            textBox2.Focus();
                            break;
                    }
                }
                else
                {
                    musteriOlustur();
                    textBox2.Focus();
                }
            }
        }

        private void comboBox2_TextUpdate(object sender, EventArgs e)
        {
            comboBox2.ResetText();
        }

        private void listBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (linkLabel3.Text == "Doðrula")
                {
                    MessageBox.Show("Bu iþlem izinsiz yapýlamaz.", "Þifreyi Doðrula", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    if (customers.Count == 0)
                    {
                        MessageBox.Show("Listede deðer bulunmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {
                        try
                        {
                            if (!checkBox13.Checked)
                            {
                                DialogResult musteriSil = MessageBox.Show("Müþteri: " + listBox3.SelectedItem.ToString() + Environment.NewLine + "Silmek istediðinizden emin misiniz?", "Müþteri Sil", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                                switch (musteriSil)
                                {
                                    case DialogResult.Cancel: break;
                                    case DialogResult.No: break;
                                    case DialogResult.Yes:
                                        if (customers.Contains(listBox3.SelectedItem))
                                        {
                                            customers.Remove(listBox3.SelectedItem);
                                            if (latestCustomer.Contains(listBox3.SelectedItem))
                                            {
                                                latestCustomer.Remove(listBox3.SelectedItem);
                                            }
                                            tabControl2.Enabled = false;
                                            comboBox2.Items.Remove(listBox3.SelectedItem);
                                            comboBox3.Items.Remove(listBox3.SelectedItem);
                                            latestCustomer.Remove(listBox3.SelectedItem);
                                            deletedCustomers.Add(listBox3.SelectedItem);
                                            listBox3.Items.Remove(listBox3.SelectedItem);
                                            comboBox2.ResetText();
                                            comboBox3.Text = "MÜÞTERÝ SEÇ";
                                            tabControl2.Enabled = true;
                                        }
                                        else
                                        {
                                            MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                if (customers.Contains(listBox3.SelectedItem))
                                {
                                    customers.Remove(listBox3.SelectedItem);
                                    tabControl2.Enabled = false;
                                    comboBox2.Items.Remove(listBox3.SelectedItem);
                                    comboBox3.Items.Remove(listBox3.SelectedItem);
                                    deletedCustomers.Add(listBox3.SelectedItem);
                                    latestCustomer.Remove(listBox3.SelectedItem);
                                    listBox3.Items.Remove(listBox3.SelectedItem);
                                    comboBox2.ResetText();
                                    comboBox3.Text = "MÜÞTERÝ SEÇ";
                                    tabControl2.Enabled = true;
                                }
                                else
                                {
                                    MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            if (checkBox12.Checked)
                            {
                                listBox2.Items.Clear();
                                int madde = 0;
                                foreach (string customs in customers)
                                {
                                    madde++;
                                    listBox2.Items.Add(madde.ToString() + "-)");
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Seçilmiþ bir deðer bulunmamaktadýr.", "Müþteri Sil (Hata)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else if (e.KeyCode == Keys.F)
            {
                if (listBox3.Items.Count == 0)
                {
                    MessageBox.Show("Listede seçili bir deðer bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (listBox3.SelectedIndex == -1)
                    {
                        MessageBox.Show("Listeden deðer seçmeniz gerek..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        MoveSelectedItemToTop(listBox3);
                        customers.Clear();
                        comboBox3.Items.Clear();
                        foreach (string cusAmount in listBox3.Items)
                        {
                            customers.Add(cusAmount);
                            comboBox3.Items.Add(cusAmount);
                        }
                        comboBox3.Text = "MÜÞTERÝ SEÇ";
                    }
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    if (customerFilter == true)
                    {
                        MessageBox.Show(listBox3.SelectedItem.ToString(), "Müþteri Bilgileri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox8.TextLength < 5 || textBox8.TextLength > 20 || textBox9.TextLength < 8 || textBox8.TextLength > 20)
            {
                MessageBox.Show("Geçerli karakter sayýlarý içeren yeni hesap adý veya þifresi giriniz.", "Ayarlar > Hesap > Deðiþtir (Hata)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (checkBox3.Checked && checkBox4.Checked)
            {
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                MessageBox.Show("Ýki kutucuða eski bilgiler girilemez.", "Ayarlar > Hesap > Deðiþtir (Hata)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox8.Clear();
                textBox9.Clear();
                textBox14.Clear();
            }
            else if (textBox8.Text == hesaplabel.Text && textBox9.Text == sifrelabel.Text)
            {
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                MessageBox.Show("Ýki kutucuða eski bilgiler girilemez.", "Ayarlar > Hesap > Deðiþtir (Hata)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox8.Clear();
                textBox9.Clear();
                textBox14.Clear();
            }
            else if (textBox8.Text.IndexOfAny(bannedchars) != -1)
            {
                MessageBox.Show("Panel ismi oluþturuken yasaklý karakter kullanýlmamalýdýr.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (textBox14.Text.Trim() != textBox9.Text.Trim())
            {
                textBox14.Clear();
                MessageBox.Show("Þifreyi doðrularken hata oluþtu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            else
            {
                DialogResult degistir = MessageBox.Show("Deðiþikliklerden emin misiniz? Devam ettiðiniz anda kilit sayfasýna yönlendirileceksiniz.", "Ayarlar > Hesap > Deðiþtir", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop);
                switch (degistir)
                {
                    case DialogResult.Yes:
                        hesaplabel.Text = textBox8.Text;
                        textBox11.Text = textBox8.Text;
                        sifrelabel.Text = textBox9.Text;
                        textBox12.Text = textBox9.Text;
                        MessageBox.Show("Baþarýyla deðiþtirildi.", "Ayarlar > Hesap > Deðiþtir", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bilgileriGuncelle();
                        textBox8.Clear();
                        textBox9.Clear();
                        textBox11.Clear();
                        textBox12.Clear();
                        textBox14.Clear();
                        checkBox3.Checked = false;
                        checkBox4.Checked = false;
                        panel4.Enabled = false;
                        panel3.Enabled = true;
                        tabControl1.Visible = false;
                        panel1.Visible = true;
                        break;
                    case DialogResult.None:
                        textBox4.Clear();
                        textBox3.Clear();
                        break;
                    case DialogResult.No:
                        textBox4.Clear();
                        textBox3.Clear();
                        break;
                    case DialogResult.Cancel:
                        textBox4.Clear();
                        textBox3.Clear();
                        break;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == hesaplabel.Text && textBox7.Text == sifrelabel.Text)
            {
                textBox6.Clear();
                textBox7.Clear();
                panel3.Enabled = false;
                panel4.Enabled = true;
            }
            else
            {
                textBox6.Clear();
                textBox7.Clear();
                MessageBox.Show("Eski hesap adý veya þifresi hatalý.", "Ayarlar > Hesap > Deðiþtir (Hata)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult emin = MessageBox.Show("Tüm deðiþikliklerden vazgeçmek istediðinizden emin misiniz?", "Ayarlar > Hesap > Deðiþtir", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (emin)
            {
                case DialogResult.None: break;
                case DialogResult.No: break;
                case DialogResult.Yes:
                    textBox8.Clear();
                    textBox9.Clear();
                    textBox14.Clear();
                    panel4.Enabled = false;
                    panel3.Enabled = true;
                    checkBox3.Checked = false;
                    checkBox4.Checked = false;
                    break;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox3.Checked)
            {
                textBox8.Clear();
                textBox8.Enabled = true;
            }
            else
            {
                textBox8.Enabled = false;
                textBox8.Text = hesaplabel.Text;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox4.Checked)
            {
                textBox9.Clear();
                textBox9.Enabled = true;
            }
            else
            {
                textBox9.Text = sifrelabel.Text;
                textBox9.Enabled = false;
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                comboBox2.Focus();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Focus();
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            comboBox3.Text = "MÜÞTERÝ SEÇ";
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button17.Focus();
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox7.Focus();
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.Focus();
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox8.Focus();
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox9.Focus();
            }
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox14.Focus();
            }
        }

        private void listBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listBox4_Enter(object sender, EventArgs e)
        {

        }

        private void listBox6_Enter(object sender, EventArgs e)
        {

        }

        private void listBox7_Enter(object sender, EventArgs e)
        {

        }

        private void listBox6_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listBox7_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel18.Enabled == true)
            {
                try
                {
                    if (ticket.Count <= 0)
                    {
                        MessageBox.Show("Listede deðer gözükmüyor..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    }
                    else
                    {
                        DialogResult sonuc = MessageBox.Show("Bu TICKET'ý silmek istediðinizden emin misiniz? (Bu iþlem geri alýnamaz)", "Sil (?)", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                        switch (sonuc)
                        {
                            case DialogResult.OK:
                                try
                                {
                                    int index = listBox1.SelectedIndex;
                                    ticket.RemoveAt(index);
                                    listBox1.Items.RemoveAt(index);
                                    listBox4.Items.RemoveAt(index);
                                    listBox6.Items.RemoveAt(index);
                                    listBox7.Items.RemoveAt(index);
                                }
                                catch
                                {
                                    MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                break;
                            case DialogResult.Cancel:
                                break;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Nesne seçilmedi veya bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Liste, filtreliyken hiçbir veri silemezsiniz. Onun yerine \"Listeyi Yenile\" yazýsýna týkladýktan sonra tekrar denemeniz.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void listBox4_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void listBox6_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void listBox7_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel18.Enabled == true)
            {
                try
                {
                    if (listBox1.SelectedIndex != -1) // Eðer bir öðe seçiliyse
                    {
                        try
                        {
                            int index = listBox1.SelectedIndex; // Seçili öðenin index'ini al
                            listBox7.Items[index] = comboBox4.Text.Trim(); // Seçili öðeyi güncelle
                        }
                        catch
                        {
                            MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Seçili nesne bulunamadý veya baþka bir sorun oluþtu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Liste, filtreliyken hiçbir veride deðiþiklik yapamazsýnýz. Onun yerine \"Listeyi Yenile\" yazýsýna týkladýktan sonra tekrar denemeniz.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void comboBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox4_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox4.Focus();
        }

        private void comboBox4_TextUpdate(object sender, EventArgs e)
        {
            comboBox4.Text = "Oluþturuldu";
        }

        private void MoveSelectedItemToTop(params ListBox[] listBoxes)
        {
            foreach (var listBox in listBoxes)
            {
                // Eðer listBox içinde en az bir öðe varsa ve seçili öðe en üstte deðilse
                if (listBox.SelectedIndex > 0)
                {
                    int selectedIndex = listBox.SelectedIndex;
                    object selectedItem = listBox.Items[selectedIndex];

                    // Önce seçili öðeyi çýkar
                    listBox.Items.RemoveAt(selectedIndex);

                    // En üste ekle
                    listBox.Items.Insert(0, selectedItem);

                    // Yeni konumu seçili yap
                    listBox.SelectedIndex = 0;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (linkLabel3.Text == "Doðrula")
            {
                MessageBox.Show("Bu iþlem izinsiz yapýlamaz.", "Þifreyi Doðrula", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                if (customers.Count == 0)
                {
                    MessageBox.Show("Listede deðer bulunmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    try
                    {
                        if (!checkBox13.Checked)
                        {
                            DialogResult musteriSil = MessageBox.Show("Müþteri: " + listBox3.SelectedItem.ToString() + Environment.NewLine + "Silmek istediðinizden emin misiniz?", "Müþteri Sil", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            switch (musteriSil)
                            {
                                case DialogResult.Cancel: break;
                                case DialogResult.No: break;
                                case DialogResult.Yes:
                                    if (customers.Contains(listBox3.SelectedItem))
                                    {
                                        customers.Remove(listBox3.SelectedItem);
                                        if (latestCustomer.Contains(listBox3.SelectedItem))
                                        {
                                            latestCustomer.Remove(listBox3.SelectedItem);
                                        }
                                        tabControl2.Enabled = false;
                                        comboBox2.Items.Remove(listBox3.SelectedItem);
                                        comboBox3.Items.Remove(listBox3.SelectedItem);
                                        latestCustomer.Remove(listBox3.SelectedItem);
                                        deletedCustomers.Add(listBox3.SelectedItem);
                                        listBox3.Items.Remove(listBox3.SelectedItem);
                                        comboBox2.ResetText();
                                        comboBox3.Text = "MÜÞTERÝ SEÇ";
                                        tabControl2.Enabled = true;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            if (customers.Contains(listBox3.SelectedItem))
                            {
                                customers.Remove(listBox3.SelectedItem);
                                tabControl2.Enabled = false;
                                comboBox2.Items.Remove(listBox3.SelectedItem);
                                comboBox3.Items.Remove(listBox3.SelectedItem);
                                deletedCustomers.Add(listBox3.SelectedItem);
                                latestCustomer.Remove(listBox3.SelectedItem);
                                listBox3.Items.Remove(listBox3.SelectedItem);
                                comboBox2.ResetText();
                                comboBox3.Text = "MÜÞTERÝ SEÇ";
                                tabControl2.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Birþeyler ters gitti.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        if (checkBox12.Checked)
                        {
                            listBox2.Items.Clear();
                            int madde = 0;
                            foreach (string customs in customers)
                            {
                                madde++;
                                listBox2.Items.Add(madde.ToString() + "-)");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Seçilmiþ bir deðer bulunmamaktadýr.", "Müþteri Sil (Hata)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox3.Items.Count == 0)
            {
                MessageBox.Show("Listede seçili bir deðer bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (listBox3.SelectedIndex == -1)
                {
                    MessageBox.Show("Listeden deðer seçmeniz gerek..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MoveSelectedItemToTop(listBox3);
                    customers.Clear();
                    comboBox3.Items.Clear();
                    foreach (string cusAmount in listBox3.Items)
                    {
                        customers.Add(cusAmount);
                        comboBox3.Items.Add(cusAmount);
                    }
                    comboBox3.Text = "MÜÞTERÝ SEÇ";
                }
            }
        }

        private void linkLabel3_LinkClicked_2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel3.Text == "Doðrula")
            {
                if (textBox10.Text.Trim() == sifrelabel.Text.Trim())
                {
                    textBox10.Enabled = false;
                    textBox10.Clear();
                    linkLabel3.Text = "Kilitle";
                    label9.Text = "";
                    label9.Text = zaman.ToString();
                    timer2.Start();
                }
                else
                {
                    textBox10.Clear();
                    MessageBox.Show("Þifre hatalý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                timer2.Stop();
                textBox10.Enabled = true;
                linkLabel3.Text = "Doðrula";
                label9.Text = "";
                zaman = 180;
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (linkLabel3.Text == "Doðrula")
                {
                    if (textBox10.Text.Trim() == sifrelabel.Text.Trim())
                    {
                        textBox10.Enabled = false;
                        textBox10.Clear();
                        linkLabel3.Text = "Kilitle";
                        label9.Text = "";
                        label9.Text = zaman.ToString();
                        timer2.Start();
                    }
                    else
                    {
                        textBox10.Clear();
                        MessageBox.Show("Þifre hatalý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    timer2.Stop();
                    textBox10.Enabled = true;
                    linkLabel3.Text = "Doðrula";
                    label9.Text = "";
                    zaman = 180;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (customers.Count == 0)
            {
                MessageBox.Show("Listede müþteri bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    customerFilter = false;
                    listBox3.Items.Clear();
                    listBox2.Items.Clear();
                    int madde = 0;
                    foreach (string cus in customers)
                    {
                        if (!checkBox12.Checked)
                        {
                            listBox3.Items.Add(cus);
                        }
                        else
                        {
                            madde++;
                            listBox2.Items.Add(madde.ToString() + "-)");
                            listBox3.Items.Add(cus);
                        }
                    }
                    linkLabel7.Enabled = true;
                    button5.Enabled = true;
                    button6.Enabled = true;
                    button17.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Bir sýkýntý oluþtu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel9_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (customers.Count > 0)
                {
                    comboBox2.Items.Clear();
                    foreach (string cus in customers)
                    {
                        comboBox2.Items.Add(cus);
                    }
                }
                else
                {
                    MessageBox.Show("Listede müþteri bulunamadý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox2.Items.Clear();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Bir sorun çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Visible = false;
            panel5.Visible = true;
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox11.Text) || !string.IsNullOrWhiteSpace(textBox12.Text) || !string.IsNullOrWhiteSpace(textBox13.Text))
            {
                DialogResult soru = MessageBox.Show("Deðiþikliklerden vazgeçmek üzeresiniz.", "Yeni Panel Oluþtur", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                switch (soru)
                {
                    case DialogResult.OK:
                        textBox11.Clear();
                        textBox12.Clear();
                        textBox13.Clear();
                        checkBox6.Checked = false;
                        panel5.Visible = false;
                        panel1.Visible = true;
                        break;
                    case DialogResult.Cancel: break;
                    case DialogResult.None: break;
                }
            }
            else
            {
                checkBox6.Checked = false;
                panel5.Visible = false;
                panel1.Visible = true;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                textBox12.PasswordChar = textBox11.PasswordChar;
            }
            else
            {
                textBox12.PasswordChar = '*';
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox12.Focus();
            }
        }

        private void textBox12_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox13.Focus();
            }
        }

        private void textBox13_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                linkLabel13.Focus();
            }
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (textBox11.TextLength < 3 || textBox11.TextLength > 20)
            {
                MessageBox.Show("Geçersiz karakter sayýsý. (3-20)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox12.TextLength < 8 || textBox12.TextLength > 20)
            {
                MessageBox.Show("Geçersiz karakter sayýsý. (8-20)", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox13.Text != textBox12.Text)
            {
                MessageBox.Show("Hatalý þifre onayý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox11.Text.IndexOfAny(bannedchars) != -1)
            {
                MessageBox.Show("Panel ismi oluþturuken yasaklý karakter kullanýlmamalýdýr.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Panel Adý: @" + textBox11.Text.Trim() + "\nÞifre: " + textBox12.Text.Trim(), "Önizleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
            {
                if (textBox11.TextLength < 3 || textBox11.TextLength > 20 || textBox12.TextLength < 8 || textBox12.TextLength > 20 || textBox13.Text != textBox12.Text)
                {
                    MessageBox.Show("Hata oluþtu, lütfen \"Önizleme\" yazýsýna týklayýp kontrol ediniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (textBox11.Text == hesaplabel.Text)
                {
                    MessageBox.Show("Ayný isme sahip TICKET paneli oluþturamazsýnýz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (textBox11.Text.IndexOfAny(bannedchars) != -1)
                {
                    MessageBox.Show("Panel ismi oluþturuken yasaklý karakter kullanýlmamalýdýr.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    DialogResult soru = MessageBox.Show("Panel Adý: @" + textBox11.Text.Trim() + "\nÞifre: " + textBox12.Text.Trim() + "\nDevam etmek istediðinizden emin misiniz?", "Yeni Panel Oluþtur (?)", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (soru)
                    {
                        case DialogResult.None: break;
                        case DialogResult.No: break;
                        case DialogResult.Cancel: break;
                        case DialogResult.Yes:
                            panel7.Enabled = false;
                            panel6.Visible = true;
                            panel6.Enabled = true;
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("\"Kabul ediyorum\" kutucuðu doldurmadan yeni panel oluþturulamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            try
            {
                label15.Text = "Kontroller yapýlýyor... (" + progressBar4.Value.ToString() + "/" + progressBar4.Maximum.ToString() + ")";
                progressBar4.Value++;
            }
            catch
            {
                timer3.Stop();
                label15.Visible = false;
                progressBar4.Visible = false;
                progressBar4.Value = 0;
                panelOlustur();
                panel6.Visible = false;
                panel7.Enabled = true;
                panel5.Enabled = true;
                panel5.Visible = false;
                panel1.Visible = true;
                textBox11.Clear();
                textBox12.Clear();
                textBox13.Clear();
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                MessageBox.Show("Yeni TICKET paneli baþarýyla oluþturuldu!\nDosya Yolu: " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ticket Panel Info"), "Ýþlem baþarýlý!");
            }
        }

        private void linkLabel17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel6.Visible = false;
            panel7.Enabled = true;
        }

        private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel6.Enabled = false;
            panel5.Enabled = false;
            label15.Visible = true;
            progressBar4.Visible = true;
            timer3.Start();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Ticket Panel Info");
            MessageBox.Show("Bilgisayarýnýzdan þu dosya yolu ile panel giriþ verilerine ulaþabilirsiniz..\n" + folderPath, "Dosya Yolu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (customers.Count > 0)
                {
                    comboBox5.Items.Clear();
                    foreach (string cus in customers)
                    {
                        comboBox5.Items.Add(cus);
                    }
                }
                else
                {
                    MessageBox.Show("Listede müþteri bulunamadý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    comboBox5.Items.Clear();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Bir sorun çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            try
            {
                if (linkLabel18.Enabled == true)
                {
                    MessageBox.Show(ticket[listBox1.SelectedIndex] + "\nTalep Durumu: " + listBox7.Items[listBox1.SelectedIndex], "TICKET Bilgileri");
                }
                else
                {
                    MessageBox.Show(listBox1.SelectedItem.ToString(), "TICKET Bilgileri");
                }
            }
            catch
            {
                return;
            }
        }

        private void listBox1_SelectedIndexChanged_2(object sender, EventArgs e)
        {

        }

        private void listBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    try
                    {
                        if (linkLabel18.Enabled == true)
                        {
                            MessageBox.Show(ticket[listBox1.SelectedIndex] + "\nTalep Durumu: " + listBox7.Items[listBox1.SelectedIndex], "TICKET Bilgileri");
                        }
                        else
                        {
                            MessageBox.Show(listBox1.SelectedItem.ToString(), "TICKET Bilgileri");
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void comboBox5_TextUpdate(object sender, EventArgs e)
        {
            comboBox5.ResetText();
        }

        private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBox5.Text.Trim()) && listBox1.Items.Count != 0)
            {
                try
                {
                    linkLabel18.Enabled = false;
                    listBox8.Items.Clear();
                    listBox8.Items.AddRange(listBox1.Items);
                    listBox1.Items.Clear();
                    ArrayList filtered = new ArrayList();
                    foreach (string filters in ticket)
                    {
                        if (filters.Contains(comboBox5.Text.Trim()))
                        {
                            filtered.Add(filters);
                        }
                    }
                    foreach (string filters in filtered)
                    {
                        listBox1.Items.Add(filters.Trim());
                    }
                    linkLabel19.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Seçenek kutusu boþ, ya da \"Filtreleme\" listesinde hiç eleman yok.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void linkLabel19_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkLabel19.Enabled = false;
                listBox1.Items.Clear();
                listBox1.Items.AddRange(listBox8.Items);
                listBox8.Items.Clear();
                linkLabel18.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Bir sýkýntý oluþtu..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked == true)
            {
                dateTimePicker3.Enabled = true;
            }
            else
            {
                dateTimePicker3.Enabled = false;
                dateTimePicker1.ResetText();
            }
        }

        private void linkLabel20_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (latestCustomer.Count != 0)
            {
                if (!customerFilter)
                {
                    DialogResult soru = MessageBox.Show("Listeyi baþtan yazar, baþa aldýklarýnýz tekrar sýralanýr.", "Uyarý", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    switch (soru)
                    {
                        case DialogResult.OK:
                            if (!checkBox12.Checked)
                            {
                                listBox2.Items.Clear();
                                listBox3.Items.Clear();
                                foreach (string item in latestCustomer)
                                {
                                    listBox3.Items.Add(item);
                                }
                            }
                            else
                            {
                                listBox2.Items.Clear();
                                listBox3.Items.Clear();
                                int madde = 0;
                                foreach (string item in latestCustomer)
                                {
                                    madde++;
                                    listBox2.Items.Add(madde.ToString() + "-)");
                                    listBox3.Items.Add(item);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("ArrayList zaten boþ gibi gözüküyor..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            try
            {
                int customerCount = customers.Count;
                progressBar3.Value = customerCount;
                label17.Text = "Kayýtlý Müþteri Sayýsý: " + progressBar3.Value.ToString() + "/" + progressBar3.Maximum.ToString();
            }
            catch
            {
                return;
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                int cusAmount = customers.Count;
                if (cusAmount == 0)
                {
                    MessageBox.Show("Müþteri bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (comboBox3.Text == "MÜÞTERÝ SEÇ")
                    {
                        MessageBox.Show("Müþteri seçilmesi lazým.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        try
                        {
                            customerFilter = true;
                            if (customers.Contains(comboBox3.Text.Trim()))
                            {
                                linkLabel7.Enabled = false;
                                button5.Enabled = false;
                                button6.Enabled = false;
                                button17.Enabled = false;
                                listBox3.Items.Clear();
                                int filterAmount = 0;
                                if (!checkBox12.Checked)
                                {
                                    foreach (string filters in ticket)
                                    {
                                        if (filters.Contains(comboBox3.Text))
                                        {
                                            filterAmount++;
                                        }
                                    }
                                    listBox3.Items.Add(comboBox3.Text + " (" + filterAmount.ToString() + ")");
                                }
                                else
                                {
                                    foreach (string filters in ticket)
                                    {
                                        if (filters.Contains(comboBox3.Text))
                                        {
                                            filterAmount++;
                                        }
                                    }
                                    listBox2.Items.Clear();
                                    listBox2.Items.Add("1-)");
                                    listBox3.Items.Add(comboBox3.Text + " (" + filterAmount.ToString() + ")");
                                }
                            }
                            else
                            {
                                customerFilter = false;
                                MessageBox.Show("Aradýðýnýz müþteri bulunamamýþtýr. \"Kutuyu Yenile\" yazýsýna týklayýp tekrar deneyebilirsiniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        catch
                        {
                            return;
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Birþeyler ters gitti..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            musteriArrayList();
        }

        private void linkLabel21_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            radioButton1.Checked = true;
            radioButton2.Checked = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox3.SelectedIndex != -1)
                {
                    listBox2.SelectedIndex = listBox3.SelectedIndex;
                    MessageBox.Show("");
                }
            }
            catch
            {
                return;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedIndex != -1)
                {
                    listBox3.SelectedIndex = listBox2.SelectedIndex;
                }
            }
            catch
            {
                return;
            }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked)
            {
                groupBox19.Enabled = true;
                groupBox20.Enabled = true;
            }
            else
            {
                groupBox19.Enabled = false;
                groupBox20.Enabled = false;
            }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked)
            {
                groupBox17.Enabled = true;
            }
            else
            {
                groupBox17.Enabled = false;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Varsayýlan ayarlara geçmek istediðinizden emin misiniz?", "Uyarý", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (sonuc)
            {
                case DialogResult.Yes:
                    checkBox14.Checked = true;
                    checkBox15.Checked = false;
                    checkBox16.Checked = false;
                    checkBox18.Checked = false;
                    break;
            }
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button8.Focus();
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            numericUpDown1.Value = customers.Count;
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            if (deletedCustomers.Count == 0)
            {
                MessageBox.Show("Liste zaten boþ gibi gözüküyor..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            else
            {
                if (checkBox19.Checked)
                {
                    listBox5.Items.Clear();
                    deletedCustomers.Clear();
                    listBox5.Items.Add("Liste boþ...");
                }
                else
                {
                    DialogResult soru = MessageBox.Show("Silinen tüm müþteriler ortadan kaybolacak, bu iþlem geri alýnamaz!", "Uyarý", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                    switch (soru)
                    {
                        case DialogResult.OK:
                            listBox5.Items.Clear();
                            deletedCustomers.Clear();
                            listBox5.Items.Add("Liste boþ...");
                            break;
                    }
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (deletedCustomers.Count == 0)
            {
                MessageBox.Show("Liste boþ gibi gözüküyor..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
            else
            {
                if (!checkBox21.Checked)
                {
                    listBox5.Items.Clear();
                    if (!checkBox20.Checked)
                    {
                        foreach (string customers in deletedCustomers)
                        {
                            listBox5.Items.Add(customers);
                        }
                    }
                    else
                    {
                        int madde = 0;
                        foreach (string customers in deletedCustomers)
                        {
                            madde++;
                            listBox5.Items.Add(madde.ToString() + "-) " + customers);
                        }
                    }
                }
                else
                {
                    if (!checkBox20.Checked)
                    {
                        listBox5.Items.Clear();
                        listBox5.Items.Add(deletedCustomers[deletedCustomers.Count - 1]);
                    }
                    else
                    {
                        listBox5.Items.Clear();
                        listBox5.Items.Add("1-) " + (deletedCustomers[deletedCustomers.Count - 1]));
                    }
                }
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (deletedCustomers.Count == 0)
            {
                MessageBox.Show("Liste boþ gibi gözüküyor..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            else
            {
                try
                {
                    string customIndex = (string)deletedCustomers[listBox5.SelectedIndex];
                    if (customers.Contains(customIndex.ToLower()) || latestCustomer.Contains(customIndex.ToLower()))
                    {
                        MessageBox.Show("Aktif bir müþteri ArrayList'te kurtarmaya çalýþtýðýnýz müþterilerden birine zaten sahip.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        return;
                    }
                    else
                    {
                        DialogResult soru = MessageBox.Show("\"" + customIndex + "\" müþterisini geri eklemek istediðinizden emin misiniz?", "Müþteri kurtar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        switch (soru)
                        {
                            case DialogResult.Yes:
                                try
                                {
                                    customers.Add(customIndex);
                                    latestCustomer.Add(customIndex);
                                    deletedCustomers.Remove(customIndex);
                                    numericUpDown1.Value = latestCustomer.Count;
                                }
                                catch
                                {
                                    MessageBox.Show("Ýþlem baþarýsýz..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                                break;
                        }
                        if (!checkBox20.Checked)
                        {
                            listBox5.Items.Clear();
                            if (deletedCustomers.Count == 0)
                            {
                                listBox5.Items.Add("Liste boþ...");
                            }
                            else
                            {
                                foreach (string item in deletedCustomers)
                                {
                                    listBox5.Items.Add(item);
                                }
                            }
                        }
                        else
                        {
                            listBox5.Items.Clear();
                            if (deletedCustomers.Count == 0)
                            {
                                listBox5.Items.Add("Liste boþ...");
                            }
                            else
                            {
                                int madde = 0;
                                foreach (string item in deletedCustomers)
                                {
                                    madde++;
                                    listBox5.Items.Add(madde.ToString() + "-) " + item);
                                }
                            }
                        }
                        if (checkBox21.Checked)
                        {
                            if (!checkBox20.Checked)
                            {
                                listBox5.Items.Clear();
                                listBox5.Items.Add(deletedCustomers[deletedCustomers.Count - 1]);
                            }
                            else
                            {
                                listBox5.Items.Clear();
                                listBox5.Items.Add("1-) " + (deletedCustomers[deletedCustomers.Count - 1]));
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Kurtarmak için deðer seçmelisiniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            checkBox19.Checked = false;
            checkBox20.Checked = true;
            checkBox21.Checked = false;
        }

        private void comboBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (comboBox7.Text == "Sistemi kilitle")
            {
                comboBox7.Text = "Sistem";
                DialogResult cikis = MessageBox.Show("Paneli kilitleyeceksiniz.", "Panel Kilitle", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                switch (cikis)
                {
                    case DialogResult.OK:
                        tabControl1.Visible = false;
                        panel1.Visible = true;
                        textBox4.Focus();
                        break;
                }
            }
            else if (comboBox7.Text == "Sistemden çýk")
            {
                comboBox7.Text = "Sistem";
                DialogResult sonuc = MessageBox.Show("Çýkýþ yapýyorsunuz.", "Panel Çýkýþ", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                switch (sonuc)
                {
                    case DialogResult.OK:
                        Application.Exit();
                        break;
                }
            }
            else
            {
                comboBox7.Text = "Sistem";
                return;
            }
        }

        private void comboBox7_TextChanged(object sender, EventArgs e)
        {
            comboBox7.Text = "Sistem";
        }

        private void comboBox7_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (ticket.Count == 0)
                {
                    listBox12.Items.Clear();
                    listBox10.Items.Clear();
                    listBox10.Items.Add("Liste boþ...");
                }
                else
                {
                    if (comboBox6.Text.Trim() == "Hepsini Göster")
                    {
                        listBox10.Items.Clear();
                        listBox12.Items.Clear();
                        int count = 0;
                        foreach (string item in beklemede)
                        {
                            count++;
                            listBox10.Items.Add(item);
                            listBox12.Items.Add("(" + count.ToString() + ")");
                        }
                        foreach (string item in onemli)
                        {
                            count++;
                            listBox10.Items.Add(item);
                            listBox12.Items.Add("(" + count.ToString() + ")");
                        }
                        foreach (string item in acil)
                        {
                            count++;
                            listBox10.Items.Add(item);
                            listBox12.Items.Add("(" + count.ToString() + ")");
                        }
                    }
                    else if (comboBox6.Text.Trim() == "Bekleyebilir")
                    {
                        listBox10.Items.Clear();
                        listBox12.Items.Clear();
                        if (beklemede.Count == 0)
                        {
                            listBox10.Items.Add("Liste boþ...");
                        }
                        else
                        {
                            int count = 0;
                            foreach (string item in beklemede)
                            {
                                count++;
                                listBox10.Items.Add(item);
                                listBox12.Items.Add("(" + count.ToString() + ")");
                            }
                        }
                    }
                    else if (comboBox6.Text.Trim() == "Önemli")
                    {
                        listBox10.Items.Clear();
                        listBox12.Items.Clear();
                        if (onemli.Count == 0)
                        {
                            listBox10.Items.Add("Liste boþ...");
                        }
                        else
                        {
                            int count = 0;
                            foreach (string item in onemli)
                            {
                                count++;
                                listBox10.Items.Add(item);
                                listBox12.Items.Add("(" + count.ToString() + ")");
                            }
                        }
                    }
                    else if (comboBox6.Text.Trim() == "Acil")
                    {
                        listBox10.Items.Clear();
                        listBox12.Items.Clear();
                        if (acil.Count == 0)
                        {
                            listBox10.Items.Add("Liste boþ...");
                        }
                        else
                        {
                            int count = 0;
                            foreach (string item in acil)
                            {
                                count++;
                                listBox10.Items.Add(item);
                                listBox12.Items.Add("(" + count.ToString() + ")");
                            }
                        }
                    }
                    else
                    {
                        comboBox6.Text = "Durum Seçiniz...";
                        MessageBox.Show("Lütfen geçerli bir seçeneðe týklayýnýz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch
            {
                comboBox6.Text = "Durum Seçiniz...";
                listBox10.Items.Clear();
                listBox10.Items.Add("Liste boþ...");
                MessageBox.Show("Filtrelerken bir hata oluþtu..\nBunun nedeni, seçtiðiniz filtrede eleman bulunmamasý olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void listBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                label11.Text = listBox10.SelectedItem.ToString();
                if (listBox10.SelectedIndex != -1)
                {
                    listBox12.SelectedIndex = listBox10.SelectedIndex;
                    numericUpDown2.Value = listBox10.SelectedIndex + 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void listBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox12.SelectedIndex != -1)
                {
                    listBox10.SelectedIndex = listBox12.SelectedIndex;
                }
            }
            catch
            {
                return;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox10.Items.Count == 1 && listBox10.Items[0] == "Liste boþ...")
                {
                    if (numericUpDown2.Value > listBox10.Items.Count || numericUpDown2.Value < listBox10.Items.Count || numericUpDown2.Value == listBox10.Items.Count)
                    {
                        numericUpDown2.Value = 0;
                        listBox10.SelectedIndex = -1;
                        listBox12.SelectedIndex = -1;
                    }
                }
                else
                {
                    if (numericUpDown2.Value == 0)
                    {
                        numericUpDown2.Value = listBox10.Items.Count;
                        int numCount = Convert.ToInt32(numericUpDown2.Value);
                        listBox10.SelectedIndex = numCount - 1;
                        listBox12.SelectedIndex = listBox10.SelectedIndex;
                        label11.Text = listBox10.SelectedItem.ToString();
                    }
                    else if (numericUpDown2.Value > listBox10.Items.Count)
                    {
                        numericUpDown2.Value = 1;
                        int numCount = Convert.ToInt32(numericUpDown2.Value);
                        listBox10.SelectedIndex = numCount - 1;
                        listBox12.SelectedIndex = listBox10.SelectedIndex;
                        label11.Text = listBox10.SelectedItem.ToString();
                    }
                    else
                    {
                        int numCount = Convert.ToInt32(numericUpDown2.Value);
                        listBox10.SelectedIndex = numCount - 1;
                        listBox12.SelectedIndex = listBox10.SelectedIndex;
                        label11.Text = listBox10.SelectedItem.ToString();
                    }
                }
            }
            catch
            {
                numericUpDown2.Value = 0;
                listBox10.SelectedIndex = -1;
                listBox12.SelectedIndex = -1;
                return;
            }
        }

        private void numericUpDown2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                numericUpDown1.Value = 0;
                listBox10.SelectedIndex = -1;
                listBox12.SelectedIndex = -1;
                return;
            }
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            if (beklemede.Count == 0)
            {
                listBox11.Items.Clear();
                listBox13.Items.Clear();
                listBox11.Items.Add("Liste boþ...");
            }
            else
            {
                listBox11.Items.Clear();
                listBox13.Items.Clear();
                int count = 0;
                foreach (string item in beklemede)
                {
                    count++;
                    listBox11.Items.Add(item);
                    listBox13.Items.Add("(" + count.ToString() + ")");
                }
            }
        }

        private void listBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                label18.Text = listBox11.SelectedItem.ToString();
                if (listBox11.SelectedIndex != -1)
                {
                    listBox13.SelectedIndex = listBox11.SelectedIndex;
                    numericUpDown3.Value = listBox11.SelectedIndex + 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void listBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox13.SelectedIndex != -1)
            {
                listBox11.SelectedIndex = listBox13.SelectedIndex;
            }
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox11.Items.Count == 1 && listBox11.Items[0] == "Liste boþ...")
                {
                    if (numericUpDown3.Value > listBox11.Items.Count || numericUpDown3.Value < listBox11.Items.Count || numericUpDown3.Value == listBox11.Items.Count)
                    {
                        numericUpDown3.Value = 0;
                        listBox11.SelectedIndex = -1;
                        listBox13.SelectedIndex = -1;
                    }
                }
                else
                {
                    if (numericUpDown3.Value == 0)
                    {
                        numericUpDown3.Value = listBox11.Items.Count;
                        int numCount = Convert.ToInt32(numericUpDown3.Value);
                        listBox11.SelectedIndex = numCount - 1;
                        listBox13.SelectedIndex = listBox11.SelectedIndex;
                        label18.Text = listBox11.SelectedItem.ToString();
                    }
                    else if (numericUpDown3.Value > beklemede.Count)
                    {
                        numericUpDown3.Value = 1;
                        int numCount = Convert.ToInt32(numericUpDown3.Value);
                        listBox11.SelectedIndex = numCount - 1;
                        listBox13.SelectedIndex = listBox11.SelectedIndex;
                        label18.Text = listBox11.SelectedItem.ToString();
                    }
                    else
                    {
                        int numCount = Convert.ToInt32(numericUpDown3.Value);
                        listBox11.SelectedIndex = numCount - 1;
                        listBox13.SelectedIndex = listBox11.SelectedIndex;
                        label18.Text = listBox11.SelectedItem.ToString();
                    }
                }
            }
            catch
            {
                numericUpDown3.Value = 0;
                listBox11.SelectedIndex = -1;
                listBox13.SelectedIndex = -1;
                return;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (listBox11.SelectedIndex == -1)
            {
                MessageBox.Show("Seçili bir veri bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (comboBox8.Text == "Önemli")
                {
                    try
                    {
                        string customIndex = (string)beklemede[listBox11.SelectedIndex];
                        if (customIndex.Contains("(Bekleyebilir)"))
                        {
                            onemli.Add(customIndex.Replace("(Bekleyebilir)", "(Önemli)"));
                            beklemede.RemoveAt(listBox11.SelectedIndex);
                            listBox11.Items.Clear();
                            listBox13.Items.Clear();
                            int count = 0;
                            foreach (string item in beklemede)
                            {
                                count++;
                                listBox11.Items.Add(item);
                                listBox13.Items.Add("(" + count.ToString() + ")");
                            }
                            numericUpDown3.Value = 0;
                            label18.ResetText();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (comboBox8.Text == "Acil")
                {
                    try
                    {
                        string customIndex = (string)beklemede[listBox11.SelectedIndex];
                        if (customIndex.Contains("(Bekleyebilir)"))
                        {
                            ;
                            acil.Add(customIndex.Replace("(Bekleyebilir)", "(Acil)"));
                            beklemede.RemoveAt(listBox11.SelectedIndex);
                            listBox11.Items.Clear();
                            listBox13.Items.Clear();
                            int count = 0;
                            foreach (string item in beklemede)
                            {
                                count++;
                                listBox11.Items.Add(item);
                                listBox13.Items.Add("(" + count.ToString() + ")");
                            }
                            numericUpDown3.Value = 0;
                            label18.ResetText();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Durum seçmeden iþlem yapamazsýnýz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox14.Items.Count == 1 && listBox14.Items[0] == "Liste boþ...")
                {
                    if (numericUpDown4.Value > listBox14.Items.Count || numericUpDown4.Value < listBox14.Items.Count || numericUpDown4.Value == listBox14.Items.Count)
                    {
                        numericUpDown4.Value = 0;
                        listBox14.SelectedIndex = -1;
                        listBox15.SelectedIndex = -1;
                    }
                }
                else
                {
                    if (numericUpDown4.Value == 0)
                    {
                        numericUpDown4.Value = listBox14.Items.Count;
                        int numCount = Convert.ToInt32(numericUpDown4.Value);
                        listBox14.SelectedIndex = numCount - 1;
                        listBox15.SelectedIndex = listBox14.SelectedIndex;
                        label19.Text = listBox14.SelectedItem.ToString();
                    }
                    else if (numericUpDown4.Value > onemli.Count)
                    {
                        numericUpDown4.Value = 1;
                        int numCount = Convert.ToInt32(numericUpDown4.Value);
                        listBox14.SelectedIndex = numCount - 1;
                        listBox15.SelectedIndex = listBox14.SelectedIndex;
                        label19.Text = listBox14.SelectedItem.ToString();
                    }
                    else
                    {
                        int numCount = Convert.ToInt32(numericUpDown4.Value);
                        listBox14.SelectedIndex = numCount - 1;
                        listBox15.SelectedIndex = listBox14.SelectedIndex;
                        label19.Text = listBox14.SelectedItem.ToString();
                    }
                }
            }
            catch
            {
                numericUpDown4.Value = 0;
                listBox14.SelectedIndex = -1;
                listBox15.SelectedIndex = -1;
                return;
            }
        }

        private void listBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                label19.Text = listBox14.SelectedItem.ToString();
                if (listBox14.SelectedIndex != -1)
                {
                    listBox15.SelectedIndex = listBox14.SelectedIndex;
                    numericUpDown4.Value = listBox14.SelectedIndex + 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void listBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox15.SelectedIndex != -1)
            {
                listBox14.SelectedIndex = listBox15.SelectedIndex;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (onemli.Count == 0)
            {
                listBox14.Items.Clear();
                listBox15.Items.Clear();
                listBox14.Items.Add("Liste boþ...");
            }
            else
            {
                listBox14.Items.Clear();
                listBox15.Items.Clear();
                int count = 0;
                foreach (string item in onemli)
                {
                    count++;
                    listBox14.Items.Add(item);
                    listBox15.Items.Add("(" + count.ToString() + ")");
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (listBox14.SelectedIndex == -1)
            {
                MessageBox.Show("Seçili bir veri bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (comboBox9.Text == "Bekleyebilir")
                {
                    try
                    {
                        string customIndex = (string)onemli[listBox14.SelectedIndex];
                        if (customIndex.Contains("(Önemli)"))
                        {
                            beklemede.Add(customIndex.Replace("(Önemli)", "(Bekleyebilir)"));
                            onemli.RemoveAt(listBox14.SelectedIndex);
                            listBox14.Items.Clear();
                            listBox15.Items.Clear();
                            int count = 0;
                            foreach (string item in onemli)
                            {
                                count++;
                                listBox14.Items.Add(item);
                                listBox15.Items.Add("(" + count.ToString() + ")");
                            }
                            numericUpDown4.Value = 0;
                            label19.ResetText();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (comboBox9.Text == "Acil")
                {
                    try
                    {
                        string customIndex = (string)onemli[listBox14.SelectedIndex];
                        if (customIndex.Contains("(Önemli)"))
                        {
                            acil.Add(customIndex.Replace("(Önemli)", "(Acil)"));
                            onemli.RemoveAt(listBox14.SelectedIndex);
                            listBox14.Items.Clear();
                            listBox15.Items.Clear();
                            int count = 0;
                            foreach (string item in onemli)
                            {
                                count++;
                                listBox14.Items.Add(item);
                                listBox15.Items.Add("(" + count.ToString() + ")");
                            }
                            numericUpDown4.Value = 0;
                            label19.ResetText();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Durum seçmeden iþlem yapamazsýnýz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
            }
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox16.Items.Count == 1 && listBox16.Items[0] == "Liste boþ...")
                {
                    if (numericUpDown5.Value > listBox16.Items.Count || numericUpDown5.Value < listBox16.Items.Count || numericUpDown5.Value == listBox16.Items.Count)
                    {
                        numericUpDown5.Value = 0;
                        listBox16.SelectedIndex = -1;
                        listBox17.SelectedIndex = -1;
                    }
                }
                else
                {
                    if (numericUpDown5.Value == 0)
                    {
                        numericUpDown5.Value = listBox16.Items.Count;
                        int numCount = Convert.ToInt32(numericUpDown5.Value);
                        listBox16.SelectedIndex = numCount - 1;
                        listBox17.SelectedIndex = listBox16.SelectedIndex;
                        label20.Text = listBox16.SelectedItem.ToString();
                    }
                    else if (numericUpDown5.Value > acil.Count)
                    {
                        numericUpDown5.Value = 1;
                        int numCount = Convert.ToInt32(numericUpDown5.Value);
                        listBox16.SelectedIndex = numCount - 1;
                        listBox17.SelectedIndex = listBox16.SelectedIndex;
                        label20.Text = listBox16.SelectedItem.ToString();
                    }
                    else
                    {
                        int numCount = Convert.ToInt32(numericUpDown5.Value);
                        listBox16.SelectedIndex = numCount - 1;
                        listBox17.SelectedIndex = listBox16.SelectedIndex;
                        label20.Text = listBox16.SelectedItem.ToString();
                    }
                }
            }
            catch
            {
                numericUpDown5.Value = 0;
                listBox16.SelectedIndex = -1;
                listBox17.SelectedIndex = -1;
                return;
            }
        }

        private void listBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                label20.Text = listBox16.SelectedItem.ToString();
                if (listBox16.SelectedIndex != -1)
                {
                    listBox17.SelectedIndex = listBox16.SelectedIndex;
                    numericUpDown5.Value = listBox16.SelectedIndex + 1;
                }
            }
            catch
            {
                return;
            }
        }

        private void listBox17_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox17.SelectedIndex != -1)
            {
                listBox16.SelectedIndex = listBox17.SelectedIndex;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (acil.Count == 0)
            {
                listBox16.Items.Clear();
                listBox17.Items.Clear();
                listBox16.Items.Add("Liste boþ...");
            }
            else
            {
                listBox16.Items.Clear();
                listBox17.Items.Clear();
                int count = 0;
                foreach (string item in acil)
                {
                    count++;
                    listBox16.Items.Add(item);
                    listBox17.Items.Add("(" + count.ToString() + ")");
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (listBox16.SelectedIndex == -1)
            {
                MessageBox.Show("Seçili bir veri bulunamadý.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (comboBox10.Text == "Bekleyebilir")
                {
                    try
                    {
                        string customIndex = (string)acil[listBox16.SelectedIndex];
                        if (customIndex.Contains("(Acil)"))
                        {
                            beklemede.Add(customIndex.Replace("(Acil)", "(Bekleyebilir)"));
                            acil.RemoveAt(listBox16.SelectedIndex);
                            listBox16.Items.Clear();
                            listBox17.Items.Clear();
                            int count = 0;
                            foreach (string item in acil)
                            {
                                count++;
                                listBox16.Items.Add(item);
                                listBox17.Items.Add("(" + count.ToString() + ")");
                            }
                            numericUpDown5.Value = 0;
                            label20.ResetText();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (comboBox10.Text == "Önemli")
                {
                    try
                    {
                        string customIndex = (string)acil[listBox16.SelectedIndex];
                        if (customIndex.Contains("(Acil)"))
                        {
                            onemli.Add(customIndex.Replace("(Acil)", "(Önemli)"));
                            acil.RemoveAt(listBox16.SelectedIndex);
                            listBox16.Items.Clear();
                            listBox17.Items.Clear();
                            int count = 0;
                            foreach (string item in acil)
                            {
                                count++;
                                listBox16.Items.Add(item);
                                listBox17.Items.Add("(" + count.ToString() + ")");
                            }
                            numericUpDown5.Value = 0;
                            label20.ResetText();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Bir sýkýntý çýktý..", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Durum seçmeden iþlem yapamazsýnýz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    return;
                }
            }
        }

        private void listBox3_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (customerFilter == true)
                {
                    MessageBox.Show(listBox3.SelectedItem.ToString(), "Müþteri Bilgileri", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    return;
                }
            }
            catch
            {
                return;
            }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox17.Checked)
            {
                textBox14.PasswordChar = '*';
            }
            else
            {
                textBox14.PasswordChar = textBox11.PasswordChar;
            }
        }

        private void textBox14_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button10.Focus();
            }
        }
    }
}
