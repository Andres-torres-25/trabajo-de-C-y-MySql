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
    public partial class PacientesUI : Form
    {
        private PacienteDAO dao = new PacienteDAO();
        private EPSDAO epsDao = new EPSDAO();
        private Panel pnlHeader;
        private Label btnCerrar;
        private Label lblTittle;
        private Label lblID;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblTeléfono;
        private Label lblEmail;
        private Label lblDirección;
        private ComboBox cbxGénero;
        private ComboBox cbxEPS;
        private DateTimePicker dtFechaNacimiento;
        private DateTimePicker dtFechaRegistro;
        private Label lblFechaDeNacimiento;
        private Label lblFechaDeRegistro;
        private Label lblGénero;
        private Label lblEPS;
        private PictureBox picFoto;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnEliminar;
        private TextBox txtID;
        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtTeléfono;
        private TextBox txtEmail;
        private TextBox txtDireccion;
        private bool isRequiredID = false;
        private object txtBuscar;
        private object txtId;
        private Label lblEdad;
        private TextBox txtEdad;
        private DataGridView dgPaciente;
        private Button btnBuscar;
        private Button btnEliminarF;
        private object dgPacientes;

        public PacientesUI()
        {
            InitializeComponent();
        }

        private void PacientesUILoad(object sender, EventArgs e)
        {
            dgPacientes.AutoGenerateColumns = false;
            CargarEPSCombo();
            CargarGrid();
            Clear();
        }

        private void CargarGrid(string filtro = "")
        {
            try
            {
                if (string.IsNullOrEmpty(filtro))
                {
                    dgPacientes.DataSource = dao.GetAll();
                }
                else if (filtro == "genero")
                {
                    dgPacientes.DataSource = dao.FiltrarPorGenero(cbxGénero.Text);
                }
                else if (filtro == "edad")
                {
                    if (int.TryParse(txtEdad.Text, out int edad))
                    {
                        dgPacientes.DataSource = dao.FiltrarPorEdad(edad);
                    }
                }
                else if (filtro == "eps")
                {
                    dgPacientes.DataSource = dao.FiltrarPorEPS((int)cbxEPS.SelectedValue);
                }
                else if (filtro == "fecha")
                {
                    dgPacientes.DataSource = dao.GetPorFechaRegistro(dtFechaNacimiento.Value.Date);
                }
                dgPacientes.Refresh();
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void CargarEPSCombo()
        {
            try
            {
                cbxEPS.DataSource = epsDao.GetAll();
                cbxEPS.DisplayMember = "Nombre";
                cbxEPS.ValueMember = "Id";
                cbxEPS.SelectedIndex = 0;

                cbxEPS.DataSource = epsDao.GetAll();
                cbxEPS.DisplayMember = "Nombre";
                cbxEPS.ValueMember = "Id";
                cbxEPS.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void Clear()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTeléfono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            cbxGénero.SelectedIndex = 0;
            cbxEPS.SelectedIndex = 0;
            dtFechaNacimiento.Value = DateTime.Now;
            dtFechaRegistro.Value = DateTime.Now;
            picFoto.Image = Properties.Resources.user1;
            isRequiredID = false;
            txtID.ReadOnly = true;
        }

        private bool IsValid()
        {
            if (isRequiredID && string.IsNullOrEmpty(txtID.Text.Trim())) return false;
            if (string.IsNullOrEmpty(txtNombre.Text.Trim())) return false;
            if (string.IsNullOrEmpty(txtApellido.Text.Trim())) return false;
            if (cbxEPS.SelectedIndex == -1) return false;
            if (picFoto.Image == null) return false;
            return true;
        }

        private Paciente? Capture()
        {
            if (!IsValid()) return null;

            Paciente obj = new Paciente
            {
                Id = isRequiredID ? Convert.ToInt32(txtID.Text) : 0,
                Nombre = txtNombre.Text.Trim().ToUpper(),
                Apellido = txtApellido.Text.Trim().ToUpper(),
                Genero = cbxGénero.Text.Substring(0, 1),
                FechaNacimiento = dtFechaNacimiento.Value,
                Telefono = txtTeléfono.Text.Trim(),
                Email = txtEmail.Text.Trim().ToUpper(),
                Direccion = txtDireccion.Text.Trim().ToUpper(),
                FechaRegistro = dtFechaRegistro.Value,
                IdEPS = (int)cbxEPS.SelectedValue
            };

            if (picFoto.Image != Properties.Resources.user1)
            {
                obj.Foto = WMImage.ImageToBytes(picFoto.Image);
            }

            return obj;
        }

        private void SaveData()
        {
            try
            {
                if (IsValid())
                {
                    if (WMHelper.Confirm("¿Desea agregar el registro?") == DialogResult.Yes)
                    {
                        dao.Add(Capture());
                        CargarGrid();
                        Clear();
                    }
                }
                else
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void UpdateData()
        {
            try
            {
                if (IsValid())
                {
                    if (WMHelper.Confirm("¿Desea actualizar el registro?") == DialogResult.Yes)
                    {
                        dao.Update(Capture());
                        CargarGrid();
                        Clear();
                    }
                }
                else
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void DeleteData()
        {
            try
            {
                if (IsValid())
                {
                    if (WMHelper.Confirm("¿Desea eliminar el registro?") == DialogResult.Yes)
                    {
                        dao.Delete(Convert.ToInt32(txtID.Text));
                        CargarGrid();
                        Clear();
                    }
                }
                else
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void FillData(int id)
        {
            try
            {
                var obj = dao.GetById(id);
                if (obj != null)
                {
                    txtID.Text = obj.Id.ToString();
                    txtNombre.Text = obj.Nombre;
                    txtApellido.Text = obj.Apellido;
                    cbxGénero.Text = obj.Genero;
                    dtFechaNacimiento.Value = obj.FechaNacimiento;
                    txtTeléfono.Text = obj.Telefono;
                    txtEmail.Text = obj.Email;
                    txtDireccion.Text = obj.Direccion;
                    dtFechaRegistro.Value = obj.FechaRegistro;
                    cbxEPS.SelectedValue = obj.IdEPS;
                    if (obj.Foto != null)
                    {
                        picFoto.Image = WMImage.BytesToImage(obj.Foto);
                    }
                    isRequiredID = true;
                    txtID.ReadOnly = false;
                }
                else
                {
                    Clear();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        // EVENTOS DE BOTONES CRUD
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnLimpiar_Click(object sender, EventArgs e) => Clear();

        // FILTROS ESPECÍFICOS
        private void btnFiltrarGenero_Click(object sender, EventArgs e)
        {
            CargarGrid("genero");
        }

        private void btnFiltrarEdad_Click(object sender, EventArgs e)
        {
            CargarGrid("edad");
        }

        private void btnFiltrarEPS_Click(object sender, EventArgs e)
        {
            CargarGrid("eps");
        }

        private void btnFiltrarFecha_Click(object sender, EventArgs e)
        {
            CargarGrid("fecha");
        }

        private void btnBuscar_Click(object sender, EventArgs e) => CargarGrid(btnBuscar.Text);

        private void CargarGrid(object text)
        {
            throw new NotImplementedException();
        }

        // SELECCIÓN EN DATAGRIDVIEW
        private void dgPacientes_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int fila = e.RowIndex;
            if (fila >= 0)
            {
                int id = Convert.ToInt32(dgPaciente[0, fila].Value);
                FillData(id);
            }
        }

        // SELECCIÓN DE FOTO
        private void picBuscarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Images (*.jpg;*.png)|*.jpg;*.png";
            if (op.ShowDialog() == DialogResult.OK)
            {
                picFoto.ImageLocation = op.FileName;
            }
        }

        private void picEliminarFoto_Click(object sender, EventArgs e)
        {
            picFoto.Image = Properties.Resources.user1;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacientesUI));
            pnlHeader = new Panel();
            btnCerrar = new Label();
            lblTittle = new Label();
            lblID = new Label();
            lblNombre = new Label();
            lblApellido = new Label();
            lblTeléfono = new Label();
            lblEmail = new Label();
            lblDirección = new Label();
            cbxGénero = new ComboBox();
            cbxEPS = new ComboBox();
            dtFechaNacimiento = new DateTimePicker();
            dtFechaRegistro = new DateTimePicker();
            lblFechaDeNacimiento = new Label();
            lblFechaDeRegistro = new Label();
            lblGénero = new Label();
            lblEPS = new Label();
            picFoto = new PictureBox();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnActualizar = new Button();
            btnEliminar = new Button();
            txtID = new TextBox();
            txtNombre = new TextBox();
            txtApellido = new TextBox();
            txtTeléfono = new TextBox();
            txtEmail = new TextBox();
            txtDireccion = new TextBox();
            lblEdad = new Label();
            txtEdad = new TextBox();
            dgPaciente = new DataGridView();
            btnBuscar = new Button();
            btnEliminarF = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFoto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgPaciente).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 115, 204);
            pnlHeader.Controls.Add(btnCerrar);
            pnlHeader.Controls.Add(lblTittle);
            pnlHeader.Cursor = Cursors.IBeam;
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(913, 50);
            pnlHeader.TabIndex = 15;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.AutoSize = true;
            btnCerrar.BackColor = Color.Red;
            btnCerrar.Font = new Font("Segoe UI", 14F);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(876, 3);
            btnCerrar.Margin = new Padding(3);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(34, 38);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "X";
            btnCerrar.Click += lblCerrar_Click;
            // 
            // lblTittle
            // 
            lblTittle.AutoSize = true;
            lblTittle.Font = new Font("Segoe UI", 14F);
            lblTittle.ForeColor = Color.White;
            lblTittle.Location = new Point(10, 10);
            lblTittle.Name = "lblTittle";
            lblTittle.Size = new Size(274, 38);
            lblTittle.TabIndex = 0;
            lblTittle.Text = "Gestión de Pacientes";
            // 
            // lblID
            // 
            lblID.ForeColor = Color.Black;
            lblID.Location = new Point(20, 70);
            lblID.Name = "lblID";
            lblID.Size = new Size(80, 25);
            lblID.TabIndex = 16;
            lblID.Text = "ID";
            // 
            // lblNombre
            // 
            lblNombre.ForeColor = Color.Black;
            lblNombre.Location = new Point(20, 104);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(93, 31);
            lblNombre.TabIndex = 17;
            lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            lblApellido.ForeColor = Color.Black;
            lblApellido.Location = new Point(20, 147);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(80, 28);
            lblApellido.TabIndex = 18;
            lblApellido.Text = "Apellido";
            // 
            // lblTeléfono
            // 
            lblTeléfono.ForeColor = Color.Black;
            lblTeléfono.Location = new Point(20, 233);
            lblTeléfono.Name = "lblTeléfono";
            lblTeléfono.Size = new Size(80, 28);
            lblTeléfono.TabIndex = 19;
            lblTeléfono.Text = "Teléfono";
            // 
            // lblEmail
            // 
            lblEmail.ForeColor = Color.Black;
            lblEmail.Location = new Point(20, 276);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(280, 25);
            lblEmail.TabIndex = 20;
            lblEmail.Text = "Email";
            // 
            // lblDirección
            // 
            lblDirección.ForeColor = Color.Black;
            lblDirección.Location = new Point(20, 316);
            lblDirección.Name = "lblDirección";
            lblDirección.Size = new Size(360, 25);
            lblDirección.TabIndex = 21;
            lblDirección.Text = "Dirección";
            // 
            // cbxGénero
            // 
            cbxGénero.FormattingEnabled = true;
            cbxGénero.Items.AddRange(new object[] { "Masculino", "Femenino", "Otros" });
            cbxGénero.Location = new Point(187, 348);
            cbxGénero.Name = "cbxGénero";
            cbxGénero.Size = new Size(150, 33);
            cbxGénero.TabIndex = 22;
            cbxGénero.Text = "Género";
            // 
            // cbxEPS
            // 
            cbxEPS.FormattingEnabled = true;
            cbxEPS.Location = new Point(187, 386);
            cbxEPS.Name = "cbxEPS";
            cbxEPS.Size = new Size(220, 33);
            cbxEPS.TabIndex = 23;
            cbxEPS.Text = "EPS";
            // 
            // dtFechaNacimiento
            // 
            dtFechaNacimiento.Location = new Point(207, 431);
            dtFechaNacimiento.Name = "dtFechaNacimiento";
            dtFechaNacimiento.Size = new Size(208, 31);
            dtFechaNacimiento.TabIndex = 24;
            // 
            // dtFechaRegistro
            // 
            dtFechaRegistro.Location = new Point(207, 468);
            dtFechaRegistro.Name = "dtFechaRegistro";
            dtFechaRegistro.Size = new Size(217, 31);
            dtFechaRegistro.TabIndex = 25;
            // 
            // lblFechaDeNacimiento
            // 
            lblFechaDeNacimiento.AutoSize = true;
            lblFechaDeNacimiento.Location = new Point(20, 434);
            lblFechaDeNacimiento.Name = "lblFechaDeNacimiento";
            lblFechaDeNacimiento.Size = new Size(181, 25);
            lblFechaDeNacimiento.TabIndex = 26;
            lblFechaDeNacimiento.Text = "Fecha de Nacimiento:";
            // 
            // lblFechaDeRegistro
            // 
            lblFechaDeRegistro.AutoSize = true;
            lblFechaDeRegistro.Location = new Point(20, 476);
            lblFechaDeRegistro.Name = "lblFechaDeRegistro";
            lblFechaDeRegistro.Size = new Size(156, 25);
            lblFechaDeRegistro.TabIndex = 27;
            lblFechaDeRegistro.Text = "Fecha de Registro:";
            // 
            // lblGénero
            // 
            lblGénero.AutoSize = true;
            lblGénero.Location = new Point(20, 356);
            lblGénero.Name = "lblGénero";
            lblGénero.Size = new Size(69, 25);
            lblGénero.TabIndex = 28;
            lblGénero.Text = "Género";
            // 
            // lblEPS
            // 
            lblEPS.AutoSize = true;
            lblEPS.Location = new Point(27, 394);
            lblEPS.Name = "lblEPS";
            lblEPS.Size = new Size(41, 25);
            lblEPS.TabIndex = 29;
            lblEPS.Text = "EPS";
            // 
            // picFoto
            // 
            picFoto.BorderStyle = BorderStyle.FixedSingle;
            picFoto.Image = (Image)resources.GetObject("picFoto.Image");
            picFoto.Location = new Point(581, 70);
            picFoto.Name = "picFoto";
            picFoto.Size = new Size(150, 156);
            picFoto.SizeMode = PictureBoxSizeMode.Zoom;
            picFoto.TabIndex = 30;
            picFoto.TabStop = false;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.FromArgb(42, 115, 204);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(505, 303);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(98, 36);
            btnNuevo.TabIndex = 33;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(42, 115, 204);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(609, 300);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(98, 36);
            btnGuardar.TabIndex = 34;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.FromArgb(42, 115, 204);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(713, 300);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(98, 36);
            btnActualizar.TabIndex = 35;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(42, 115, 204);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(609, 354);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(98, 36);
            btnEliminar.TabIndex = 36;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // txtID
            // 
            txtID.Location = new Point(187, 70);
            txtID.Name = "txtID";
            txtID.Size = new Size(188, 31);
            txtID.TabIndex = 38;
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(187, 110);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(188, 31);
            txtNombre.TabIndex = 39;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(187, 147);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(188, 31);
            txtApellido.TabIndex = 40;
            // 
            // txtTeléfono
            // 
            txtTeléfono.Location = new Point(187, 233);
            txtTeléfono.Name = "txtTeléfono";
            txtTeléfono.Size = new Size(188, 31);
            txtTeléfono.TabIndex = 41;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(187, 273);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(188, 31);
            txtEmail.TabIndex = 42;
            // 
            // txtDireccion
            // 
            txtDireccion.Location = new Point(187, 313);
            txtDireccion.Name = "txtDireccion";
            txtDireccion.Size = new Size(188, 31);
            txtDireccion.TabIndex = 43;
            // 
            // lblEdad
            // 
            lblEdad.AutoSize = true;
            lblEdad.Location = new Point(37, 201);
            lblEdad.Name = "lblEdad";
            lblEdad.Size = new Size(52, 25);
            lblEdad.TabIndex = 44;
            lblEdad.Text = "Edad";
            // 
            // txtEdad
            // 
            txtEdad.Location = new Point(187, 195);
            txtEdad.Name = "txtEdad";
            txtEdad.Size = new Size(188, 31);
            txtEdad.TabIndex = 45;
            // 
            // dgPaciente
            // 
            dgPaciente.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgPaciente.Location = new Point(474, 396);
            dgPaciente.Name = "dgPaciente";
            dgPaciente.RowHeadersWidth = 62;
            dgPaciente.Size = new Size(360, 225);
            dgPaciente.TabIndex = 46;
            // 
            // btnBuscar
            // 
            btnBuscar.BackColor = Color.FromArgb(42, 115, 204);
            btnBuscar.ForeColor = Color.White;
            btnBuscar.Location = new Point(551, 233);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(112, 34);
            btnBuscar.TabIndex = 47;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = false;
            // 
            // btnEliminarF
            // 
            btnEliminarF.BackColor = Color.Red;
            btnEliminarF.Location = new Point(669, 233);
            btnEliminarF.Name = "btnEliminarF";
            btnEliminarF.Size = new Size(112, 34);
            btnEliminarF.TabIndex = 48;
            btnEliminarF.Text = "Eliminar";
            btnEliminarF.UseVisualStyleBackColor = false;
            // 
            // PacientesUI
            // 
            ClientSize = new Size(913, 633);
            Controls.Add(btnEliminarF);
            Controls.Add(btnBuscar);
            Controls.Add(dgPaciente);
            Controls.Add(txtEdad);
            Controls.Add(lblEdad);
            Controls.Add(txtDireccion);
            Controls.Add(txtEmail);
            Controls.Add(txtTeléfono);
            Controls.Add(txtApellido);
            Controls.Add(txtNombre);
            Controls.Add(txtID);
            Controls.Add(btnEliminar);
            Controls.Add(btnActualizar);
            Controls.Add(btnGuardar);
            Controls.Add(btnNuevo);
            Controls.Add(picFoto);
            Controls.Add(lblEPS);
            Controls.Add(lblGénero);
            Controls.Add(lblFechaDeRegistro);
            Controls.Add(lblFechaDeNacimiento);
            Controls.Add(dtFechaRegistro);
            Controls.Add(dtFechaNacimiento);
            Controls.Add(cbxEPS);
            Controls.Add(cbxGénero);
            Controls.Add(lblDirección);
            Controls.Add(lblEmail);
            Controls.Add(lblTeléfono);
            Controls.Add(lblApellido);
            Controls.Add(lblNombre);
            Controls.Add(lblID);
            Controls.Add(pnlHeader);
            ForeColor = Color.Black;
            Name = "PacientesUI";
            Load += PacientesUI_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picFoto).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgPaciente).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private void lblCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PacientesUI_Load(object sender, EventArgs e)
        {

        }
    }
}
