using ClinicaApp._JP1._0.DAO;
using ClinicaApp._JP1._0.Helpers;
using ClinicaApp._JP1._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaApp._JP1._0.UI

{
    public partial class MainUI : Form
    {
        private Form? frm = null;
        private Panel pnlHeader;
        private Label lblTittle;
        private Panel pnlSidebar;
        private Panel pnlContent;
        private PictureBox picLogo;
        private StatusStrip statusStripMain;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private Button btnMinimize;
        private Button btnMaximize;
        private Button btnClose;
        private Button btnPacientes;
        private Button btnDoctores;
        private Button btnEPS;
        private Button btnEspecialidades;
        private Button btnCitas;
        private Button btnUsuarios;
        private Panel pnlContainer;
        private UsuarioDAO usuarioDao = new UsuarioDAO();

        public MainUI()
        {
            InitializeComponent();
        }

        private void MainUILoad(object sender, EventArgs e)
        {
            ShowUserData();
        }

        private void ShowUserData()
        {
            try
            {
                Usuario u = Program.UsuarioLogeado;
                btnUsuarios.Text = u.Nombre + " " + u.Apellido;

                if (u.Foto != null)
                {
                    PictureBox picFoto = new PictureBox
                    {
                        BorderStyle = BorderStyle.FixedSingle.FromArgb(21, 19, 20),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Location = new Point(81, 55),
                        Size = new Size(120, 120),
                        Image = WMImage.BytesToImage(u.Foto)
                    };
                    Controls.Add(picFoto);
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void ShowForm(Form form)
        {
            // Cerrar formulario anterior
            if (frm != null)
            {
                frm.Close();
                frm.Dispose();
            }

            // Configurar nuevo formulario
            frm = form;
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.WindowState = FormWindowState.Maximized;
            frm.Dock = DockStyle.Fill;
            pnlContainer.Controls.Add(frm);
            frm.Show();
        }

        private void btnEPS_Click(object sender, EventArgs e)
        {
            ShowForm(new EPSUI());
        }

        private void btnEspecialidades_Click(object sender, EventArgs e)
        {
            ShowForm(new EspecialidadesUI());
        }

        private void btnDoctores_Click(object sender, EventArgs e)
        {
            ShowForm(new DoctoresUI());
        }

        private void btnPacientes_Click(object sender, EventArgs e)
        {
            ShowForm(new PacientesUI());
        }

        private void btnCitas_Click(object sender, EventArgs e)
        {
            ShowForm(new CitasUI());
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            ShowForm(new UsuarioUI());
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            if (WMHelper.Confirm("¿Desea salir del sistema?") == DialogResult.Yes)
            {
                Program.UsuarioLogeado = null;
                this.Close();
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainUI));
            pnlHeader = new Panel();
            btnMinimize = new Button();
            btnMaximize = new Button();
            btnClose = new Button();
            picLogo = new PictureBox();
            lblTittle = new Label();
            pnlSidebar = new Panel();
            btnUsuarios = new Button();
            btnEspecialidades = new Button();
            btnCitas = new Button();
            btnDoctores = new Button();
            btnEPS = new Button();
            btnPacientes = new Button();
            pnlContent = new Panel();
            pnlContainer = new Panel();
            statusStripMain = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            pnlSidebar.SuspendLayout();
            pnlContent.SuspendLayout();
            statusStripMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 115, 204);
            pnlHeader.Controls.Add(btnMinimize);
            pnlHeader.Controls.Add(btnMaximize);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Controls.Add(lblTittle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1152, 60);
            pnlHeader.TabIndex = 0;
            pnlHeader.Paint += pnlHeader_Paint;
            // 
            // btnMinimize
            // 
            btnMinimize.FlatAppearance.BorderSize = 0;
            btnMinimize.FlatStyle = FlatStyle.Flat;
            btnMinimize.ForeColor = Color.White;
            btnMinimize.Location = new Point(1082, 5);
            btnMinimize.Name = "btnMinimize";
            btnMinimize.Size = new Size(58, 45);
            btnMinimize.TabIndex = 4;
            btnMinimize.Text = "x";
            btnMinimize.UseVisualStyleBackColor = true;
            btnMinimize.Click += btnMinimize_Click;
            // 
            // btnMaximize
            // 
            btnMaximize.FlatAppearance.BorderSize = 0;
            btnMaximize.FlatStyle = FlatStyle.Flat;
            btnMaximize.ForeColor = Color.White;
            btnMaximize.Location = new Point(998, -10);
            btnMaximize.Name = "btnMaximize";
            btnMaximize.Size = new Size(48, 67);
            btnMaximize.TabIndex = 3;
            btnMaximize.Text = "_";
            btnMaximize.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.White;
            btnClose.Location = new Point(1040, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 55);
            btnClose.TabIndex = 2;
            btnClose.Text = "□";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // picLogo
            // 
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            picLogo.Image = (Image)resources.GetObject("picLogo.Image");
            picLogo.Location = new Point(5, 3);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(99, 52);
            picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picLogo.TabIndex = 1;
            picLogo.TabStop = false;
            // 
            // lblTittle
            // 
            lblTittle.AutoSize = true;
            lblTittle.Font = new Font("Segoe UI", 16F);
            lblTittle.ForeColor = Color.White;
            lblTittle.Location = new Point(121, 9);
            lblTittle.Name = "lblTittle";
            lblTittle.Size = new Size(332, 45);
            lblTittle.TabIndex = 0;
            lblTittle.Text = "Clínica-Panel Principal";
            lblTittle.Click += lblTittle_Click;
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(42, 115, 204);
            pnlSidebar.Controls.Add(btnUsuarios);
            pnlSidebar.Controls.Add(btnEspecialidades);
            pnlSidebar.Controls.Add(btnCitas);
            pnlSidebar.Controls.Add(btnDoctores);
            pnlSidebar.Controls.Add(btnEPS);
            pnlSidebar.Controls.Add(btnPacientes);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 60);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(220, 488);
            pnlSidebar.TabIndex = 1;
            // 
            // btnUsuarios
            // 
            btnUsuarios.AutoSize = true;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Location = new Point(57, 17);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(10, 0, 0, 0);
            btnUsuarios.Size = new Size(118, 48);
            btnUsuarios.TabIndex = 1;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.UseVisualStyleBackColor = true;
            btnUsuarios.Click += btnUsuarios_Click;
            // 
            // btnEspecialidades
            // 
            btnEspecialidades.FlatAppearance.BorderSize = 0;
            btnEspecialidades.FlatStyle = FlatStyle.Flat;
            btnEspecialidades.ForeColor = Color.White;
            btnEspecialidades.Location = new Point(33, 284);
            btnEspecialidades.Name = "btnEspecialidades";
            btnEspecialidades.Padding = new Padding(10, 0, 0, 0);
            btnEspecialidades.Size = new Size(163, 48);
            btnEspecialidades.TabIndex = 1;
            btnEspecialidades.Text = "Especialidades";
            btnEspecialidades.TextAlign = ContentAlignment.MiddleLeft;
            btnEspecialidades.UseVisualStyleBackColor = true;
            // 
            // btnCitas
            // 
            btnCitas.FlatAppearance.BorderSize = 0;
            btnCitas.FlatStyle = FlatStyle.Flat;
            btnCitas.ForeColor = Color.White;
            btnCitas.Location = new Point(57, 214);
            btnCitas.Name = "btnCitas";
            btnCitas.Padding = new Padding(10, 0, 0, 0);
            btnCitas.Size = new Size(118, 48);
            btnCitas.TabIndex = 1;
            btnCitas.Text = "Citas";
            btnCitas.TextAlign = ContentAlignment.MiddleLeft;
            btnCitas.UseVisualStyleBackColor = true;
            btnCitas.Click += btnCitas_Click_1;
            // 
            // btnDoctores
            // 
            btnDoctores.FlatAppearance.BorderSize = 0;
            btnDoctores.FlatStyle = FlatStyle.Flat;
            btnDoctores.ForeColor = Color.White;
            btnDoctores.Location = new Point(57, 361);
            btnDoctores.Name = "btnDoctores";
            btnDoctores.Padding = new Padding(10, 0, 0, 0);
            btnDoctores.Size = new Size(118, 48);
            btnDoctores.TabIndex = 2;
            btnDoctores.Text = "Doctores";
            btnDoctores.TextAlign = ContentAlignment.MiddleLeft;
            btnDoctores.UseVisualStyleBackColor = true;
            btnDoctores.Click += button1_Click_1;
            // 
            // btnEPS
            // 
            btnEPS.FlatAppearance.BorderSize = 0;
            btnEPS.FlatStyle = FlatStyle.Flat;
            btnEPS.ForeColor = Color.White;
            btnEPS.Location = new Point(57, 146);
            btnEPS.Name = "btnEPS";
            btnEPS.Padding = new Padding(10, 0, 0, 0);
            btnEPS.Size = new Size(118, 48);
            btnEPS.TabIndex = 1;
            btnEPS.Text = "EPS";
            btnEPS.TextAlign = ContentAlignment.MiddleLeft;
            btnEPS.UseVisualStyleBackColor = true;
            btnEPS.Click += btnEPS_Click;
            // 
            // btnPacientes
            // 
            btnPacientes.FlatAppearance.BorderSize = 0;
            btnPacientes.FlatStyle = FlatStyle.Flat;
            btnPacientes.ForeColor = Color.White;
            btnPacientes.Location = new Point(57, 81);
            btnPacientes.Name = "btnPacientes";
            btnPacientes.Padding = new Padding(10, 0, 0, 0);
            btnPacientes.Size = new Size(118, 48);
            btnPacientes.TabIndex = 0;
            btnPacientes.Text = "Pacientes";
            btnPacientes.TextAlign = ContentAlignment.MiddleLeft;
            btnPacientes.UseVisualStyleBackColor = true;
            btnPacientes.Click += btnPacientes_Click_1;
            // 
            // pnlContent
            // 
            pnlContent.Controls.Add(pnlContainer);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(220, 60);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(932, 488);
            pnlContent.TabIndex = 2;
            pnlContent.Paint += pnlContent_Paint;
            // 
            // pnlContainer
            // 
            pnlContainer.Dock = DockStyle.Fill;
            pnlContainer.Location = new Point(0, 0);
            pnlContainer.Name = "pnlContainer";
            pnlContainer.Size = new Size(932, 488);
            pnlContainer.TabIndex = 0;
            pnlContainer.Paint += panel1_Paint;
            // 
            // statusStripMain
            // 
            statusStripMain.ImageScalingSize = new Size(24, 24);
            statusStripMain.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStripMain.Location = new Point(220, 516);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Size = new Size(932, 32);
            statusStripMain.TabIndex = 3;
            statusStripMain.Text = "statusStrip1";
            statusStripMain.ItemClicked += statusStripMain_ItemClicked;
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(917, 25);
            toolStripStatusLabel1.Spring = true;
            toolStripStatusLabel1.Text = "Usuario: admin | Estado: Listo";
            toolStripStatusLabel1.Click += toolStripStatusLabel1_Click;
            // 
            // MainUI
            // 
            ClientSize = new Size(1152, 548);
            Controls.Add(statusStripMain);
            Controls.Add(pnlContent);
            Controls.Add(pnlSidebar);
            Controls.Add(pnlHeader);
            Name = "MainUI";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            pnlSidebar.ResumeLayout(false);
            pnlSidebar.PerformLayout();
            pnlContent.ResumeLayout(false);
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        private void MainUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frm != null)
            {
                frm.Dispose();
            }
        }

        private void statusStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void pnlContent_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnCitas_Click_1(object sender, EventArgs e)
        {

        }

        private void btnPacientes_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTittle_Click(object sender, EventArgs e)
        {

        }
    }
}
