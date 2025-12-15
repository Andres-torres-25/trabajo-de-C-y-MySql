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
    public partial class LoginUI : Form
    {
        private UsuarioDAO dao = new UsuarioDAO();

        public LoginUI()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            // Inicializaciones adicionales (si se usa Designer, muchos de estos están ya)
            this.AcceptButton = btnAceptar;
            this.CancelButton = btnCancelar;
            txtPassword.UseSystemPasswordChar = true;
            // Opcional: establecer texto por defecto para pruebas
            // txtUsuario.Text = "admin";
            // txtPassword.Text = "admin123";
        }

        private bool IsValidInput()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                WMHelper.ShowError("El usuario es obligatorio.");
                txtUsuario.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                WMHelper.ShowError("La contraseña es obligatoria.");
                txtPassword.Focus();
                return false;
            }
            return true;
        }

        private void PerformLogin()
        {
            try
            {
                if (!IsValidInput()) return;

                string user = txtUsuario.Text.Trim();
                string pass = WMCoder.Code(txtPassword.Text.Trim()); // almacenar/consultar password hasheado

                Usuario? u = dao.Exists(user, pass);
                if (u != null)
                {
                    Program.UsuarioLogeado = u;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    WMHelper.ShowError("Usuario o contraseña inválidos.");
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError("Error al autenticar: " + ex.Message);
            }
        }

        // Eventos (asignar desde el Designer o manualmente en InitializeComponent)
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            PerformLogin();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void picShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }

        private void picShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void LoginUI_Load(object sender, EventArgs e)
        {
            // Si se requiere alguna semilla o chequeo inicial lo coloca aquí.
        }

        // Evita cerrar la aplicación si el login no fue exitoso cuando es formulario modal en Program.cs:
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Si se quiere forzar salida cuando se cancela el login y no hay usuario logeado,
            // dejar que el Program.cs lo gestione. Aquí solo un control opcional.
        }

        // Código del Designer (esquema básico si no se tiene el .Designer.cs)
        // Si se usa el diseñador de Visual Studio, este bloque no es necesario;
        // si no, copiar y adaptar el siguiente InitializeComponent mínimo:
        private Button btnAceptar;
        private Button btnCancelar;
        private TextBox txtUsuario;
        private TextBox txtPassword;
        private Label lblUsuario;
        private Label lblPassword;
        private PictureBox picShowPassword;

        private void InitializeComponent()
        {
            btnAceptar = new Button();
            btnCancelar = new Button();
            txtUsuario = new TextBox();
            txtPassword = new TextBox();
            lblUsuario = new Label();
            lblPassword = new Label();
            picShowPassword = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picShowPassword).BeginInit();
            SuspendLayout();
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(31, 145);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(120, 30);
            btnAceptar.TabIndex = 4;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(171, 145);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 30);
            btnCancelar.TabIndex = 5;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // txtUsuario
            // 
            txtUsuario.Location = new Point(31, 48);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(260, 31);
            txtUsuario.TabIndex = 1;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(31, 105);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(230, 31);
            txtPassword.TabIndex = 3;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(28, 28);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(72, 25);
            lblUsuario.TabIndex = 0;
            lblUsuario.Text = "Usuario";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(28, 85);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(101, 25);
            lblPassword.TabIndex = 2;
            lblPassword.Text = "Contraseña";
            // 
            // picShowPassword
            // 
            picShowPassword.Cursor = Cursors.Hand;
            picShowPassword.Location = new Point(265, 105);
            picShowPassword.Name = "picShowPassword";
            picShowPassword.Size = new Size(26, 22);
            picShowPassword.SizeMode = PictureBoxSizeMode.Zoom;
            picShowPassword.TabIndex = 6;
            picShowPassword.TabStop = false;
            picShowPassword.MouseDown += picShowPassword_MouseDown;
            picShowPassword.MouseUp += picShowPassword_MouseUp;
            // 
            // LoginUI
            // 
            ClientSize = new Size(324, 200);
            Controls.Add(picShowPassword);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(txtPassword);
            Controls.Add(lblPassword);
            Controls.Add(txtUsuario);
            Controls.Add(lblUsuario);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginUI";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Acceso al sistema";
            Load += LoginUI_Load;
            ((System.ComponentModel.ISupportInitialize)picShowPassword).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
