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
    public partial class DoctoresUI : Form
    {
        private DoctorDAO dao = new DoctorDAO();
        private Panel pnlHeader;
        private Label lblGestionDoctores;
        private Button btnCerrar;
        private GroupBox gbDoctores;
        private TextBox txtNombre;
        private Label lblNombre;
        private TextBox txtApellido;
        private Label lblApellido;
        private TextBox txtId;
        private TextBox txtEmail;
        private TextBox txtTelefono;
        private Label lblEmail;
        private Label lblTelefono;
        private Label lblEspecialidad;
        private Label lblHorario;
        private Label label5;
        private ComboBox cbxEspecialidad;
        private DateTimePicker dtpHorarioFin;
        private DateTimePicker dtpHorarioInicio;
        private PictureBox picFoto;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnEliminar;
        private Button btnSubirFoto;
        private Button btnEliminarFoto;
        private DataGridView dgDoctores;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colApellido;
        private DataGridViewTextBoxColumn colEspecialidad;
        private DataGridViewTextBoxColumn colTelefono;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colNombre;
        private TextBox txtFiltro;
        private TextBox txtEspecialidadFiltro;
        private Label lblFiltroNombre;
        private Label lblFiltroEspecialidad;
        private Button btnBuscar;
        private Button btnMostrarTodos;
        private EspecialidadDAO especialidadDao = new EspecialidadDAO();

        public DoctoresUI()
        {
            InitializeComponent();
        }

        private void DoctoresUI_Load(object sender, EventArgs e)
        {
            dgDoctores.AutoGenerateColumns = false;
            CargarEspecialidades();
            CargarGrid();
            Limpiar();
        }

        private void CargarEspecialidades()
        {
            try
            {
                cbxEspecialidad.DataSource = especialidadDao.GetAll();
                cbxEspecialidad.DisplayMember = "Nombre";
                cbxEspecialidad.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void CargarGrid(string filtro = "", string text = null)
        {
            try
            {
                List<Doctor> lista;
                if (string.IsNullOrEmpty(filtro))
                {
                    lista = dao.GetAll();
                }
                else
                {
                    lista = dao.GetByNombre(filtro);
                }
                dgDoctores.DataSource = lista;
                dgDoctores.Refresh();
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void Limpiar()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            cbxEspecialidad.SelectedIndex = -1;
            dtpHorarioInicio.Value = DateTime.Now;
            dtpHorarioFin.Value = DateTime.Now;
            picFoto.Image = Properties.Resources.user1;
            txtId.ReadOnly = true;
            btnGuardar.Text = "Guardar";
        }

        private bool EsValido(bool isRequiredID = false)
        {
            if (isRequiredID && string.IsNullOrEmpty(txtId.Text)) return false;
            if (string.IsNullOrEmpty(txtNombre.Text.Trim())) return false;
            if (string.IsNullOrEmpty(txtApellido.Text.Trim())) return false;
            if (string.IsNullOrEmpty(txtTelefono.Text.Trim())) return false;
            if (string.IsNullOrEmpty(txtEmail.Text.Trim())) return false;
            if (cbxEspecialidad.SelectedIndex == -1) return false;
            return true;
        }

        private Doctor Capturar(bool isRequiredID = false)
        {
            if (!EsValido(isRequiredID)) return null;

            Doctor obj = new Doctor
            {
                Id = isRequiredID ? Convert.ToInt32(txtId.Text) : 0,
                Nombre = txtNombre.Text.Trim().ToUpper(),
                Apellido = txtApellido.Text.Trim().ToUpper(),
                Telefono = txtTelefono.Text.Trim(),
                Email = txtEmail.Text.Trim().ToUpper(),
                IdEspecialidad = Convert.ToInt32(cbxEspecialidad.SelectedValue),
                HorarioInicio = dtpHorarioInicio.Value.TimeOfDay,
                HorarioFin = dtpHorarioFin.Value.TimeOfDay
            };
            return obj;
        }

        private void Guardar()
        {
            try
            {
                if (WMHelper.Confirm("¿Desea agregar el registro?") == DialogResult.Yes)
                {
                    var obj = Capturar(false);
                    if (obj != null)
                    {
                        dao.Add(obj);
                        CargarGrid();
                        Limpiar();
                        WMHelper.ShowError("¡Doctor registrado correctamente!");
                    }
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void Actualizar()
        {
            try
            {
                if (WMHelper.Confirm("¿Desea actualizar el registro?") == DialogResult.Yes)
                {
                    var obj = Capturar(true);
                    if (obj != null)
                    {
                        dao.Update(obj);
                        CargarGrid();
                        Limpiar();
                        WMHelper.ShowError("¡Doctor actualizado correctamente!");
                    }
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void Eliminar()
        {
            try
            {
                if (WMHelper.Confirm("¿Desea eliminar el registro?") == DialogResult.Yes)
                {
                    var obj = Capturar(true);
                    if (obj != null)
                    {
                        dao.Delete(obj.Id);
                        CargarGrid();
                        Limpiar();
                        WMHelper.ShowError("¡Doctor eliminado correctamente!");
                    }
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void CargarDatos(int id)
        {
            try
            {
                var obj = dao.GetById(id);
                if (obj != null)
                {
                    txtId.Text = obj.Id.ToString();
                    txtNombre.Text = obj.Nombre;
                    txtApellido.Text = obj.Apellido;
                    txtTelefono.Text = obj.Telefono;
                    txtEmail.Text = obj.Email;
                    cbxEspecialidad.SelectedValue = obj.IdEspecialidad;
                    dtpHorarioInicio.Value = DateTime.Today.Add(obj.HorarioInicio ?? TimeSpan.Zero);
                    dtpHorarioFin.Value = DateTime.Today.Add(obj.HorarioFin ?? TimeSpan.Zero);
                    btnGuardar.Text = "Actualizar";
                    txtId.ReadOnly = true;
                }
                else
                {
                    Limpiar();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        // EVENTOS - CONECTAR EN DESIGNER
        private void btnNuevo_Click(object sender, EventArgs e) => Limpiar();
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "Guardar") Guardar(); else Actualizar();
        }
        private void btnEliminar_Click(object sender, EventArgs e) => Eliminar();
        private void btnLimpiar_Click(object sender, EventArgs e) => Limpiar();
        private void btnBuscar_Click(object sender, EventArgs e) => CargarGrid(txtFiltro.Text.Trim());
        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) CargarGrid(txtFiltro.Text.Trim());
        }
        private void dgDoctores_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dgDoctores[0, e.RowIndex].Value);
                CargarDatos(id);
            }
        }
        private void picBuscarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Imágenes|*.jpg;*.png;*.bmp";
            if (op.ShowDialog() == DialogResult.OK) picFoto.ImageLocation = op.FileName;
        }
        private void picEliminarFoto_Click(object sender, EventArgs e) => picFoto.Image = Properties.Resources.user1;
        private void lblCerrar_Click(object sender, EventArgs e) => this.Close();
        private void btnFiltrarEspecialidad_Click(object sender, EventArgs e)
        {
            try
            {
                List<Doctor> lista = dao.GetByEspecialidad(txtEspecialidadFiltro.Text.Trim());
                dgDoctores.DataSource = lista;
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnCerrar = new Button();
            lblGestionDoctores = new Label();
            gbDoctores = new GroupBox();
            txtId = new TextBox();
            dtpHorarioFin = new DateTimePicker();
            label5 = new Label();
            dtpHorarioInicio = new DateTimePicker();
            lblHorario = new Label();
            cbxEspecialidad = new ComboBox();
            lblEspecialidad = new Label();
            txtEmail = new TextBox();
            txtTelefono = new TextBox();
            lblEmail = new Label();
            lblTelefono = new Label();
            txtApellido = new TextBox();
            lblApellido = new Label();
            txtNombre = new TextBox();
            lblNombre = new Label();
            picFoto = new PictureBox();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnActualizar = new Button();
            btnEliminar = new Button();
            btnSubirFoto = new Button();
            btnEliminarFoto = new Button();
            dgDoctores = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colApellido = new DataGridViewTextBoxColumn();
            colEspecialidad = new DataGridViewTextBoxColumn();
            colTelefono = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            txtFiltro = new TextBox();
            txtEspecialidadFiltro = new TextBox();
            lblFiltroNombre = new Label();
            lblFiltroEspecialidad = new Label();
            btnBuscar = new Button();
            btnMostrarTodos = new Button();
            pnlHeader.SuspendLayout();
            gbDoctores.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picFoto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgDoctores).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 115, 204);
            pnlHeader.Controls.Add(btnCerrar);
            pnlHeader.Controls.Add(lblGestionDoctores);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(772, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.BackColor = Color.FromArgb(42, 115, 204);
            btnCerrar.Cursor = Cursors.Hand;
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Font = new Font("Segoe UI", 12F);
            btnCerrar.ForeColor = Color.White;
            btnCerrar.Location = new Point(720, 3);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(40, 40);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "X";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // lblGestionDoctores
            // 
            lblGestionDoctores.AutoSize = true;
            lblGestionDoctores.Font = new Font("Segoe UI", 14F);
            lblGestionDoctores.ForeColor = Color.White;
            lblGestionDoctores.Location = new Point(12, 9);
            lblGestionDoctores.Name = "lblGestionDoctores";
            lblGestionDoctores.Size = new Size(269, 38);
            lblGestionDoctores.TabIndex = 0;
            lblGestionDoctores.Text = "Gestión de Doctores";
            // 
            // gbDoctores
            // 
            gbDoctores.Controls.Add(txtId);
            gbDoctores.Controls.Add(dtpHorarioFin);
            gbDoctores.Controls.Add(label5);
            gbDoctores.Controls.Add(dtpHorarioInicio);
            gbDoctores.Controls.Add(lblHorario);
            gbDoctores.Controls.Add(cbxEspecialidad);
            gbDoctores.Controls.Add(lblEspecialidad);
            gbDoctores.Controls.Add(txtEmail);
            gbDoctores.Controls.Add(txtTelefono);
            gbDoctores.Controls.Add(lblEmail);
            gbDoctores.Controls.Add(lblTelefono);
            gbDoctores.Controls.Add(txtApellido);
            gbDoctores.Controls.Add(lblApellido);
            gbDoctores.Controls.Add(txtNombre);
            gbDoctores.Controls.Add(lblNombre);
            gbDoctores.Location = new Point(12, 62);
            gbDoctores.Name = "gbDoctores";
            gbDoctores.Size = new Size(360, 356);
            gbDoctores.TabIndex = 1;
            gbDoctores.TabStop = false;
            gbDoctores.Text = "Datos del Doctor";
            // 
            // txtId
            // 
            txtId.Location = new Point(105, 40);
            txtId.Name = "txtId";
            txtId.Size = new Size(240, 33);
            txtId.TabIndex = 8;
            // 
            // dtpHorarioFin
            // 
            dtpHorarioFin.Format = DateTimePickerFormat.Time;
            dtpHorarioFin.Location = new Point(197, 317);
            dtpHorarioFin.Name = "dtpHorarioFin";
            dtpHorarioFin.ShowUpDown = true;
            dtpHorarioFin.Size = new Size(148, 33);
            dtpHorarioFin.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(33, 43);
            label5.Name = "label5";
            label5.Size = new Size(0, 28);
            label5.TabIndex = 8;
            // 
            // dtpHorarioInicio
            // 
            dtpHorarioInicio.Format = DateTimePickerFormat.Time;
            dtpHorarioInicio.Location = new Point(197, 278);
            dtpHorarioInicio.Name = "dtpHorarioInicio";
            dtpHorarioInicio.ShowUpDown = true;
            dtpHorarioInicio.Size = new Size(150, 33);
            dtpHorarioInicio.TabIndex = 9;
            // 
            // lblHorario
            // 
            lblHorario.AutoSize = true;
            lblHorario.Location = new Point(16, 298);
            lblHorario.Name = "lblHorario";
            lblHorario.Size = new Size(175, 28);
            lblHorario.TabIndex = 7;
            lblHorario.Text = "Horario Inicio - Fin";
            // 
            // cbxEspecialidad
            // 
            cbxEspecialidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxEspecialidad.FormattingEnabled = true;
            cbxEspecialidad.Location = new Point(141, 236);
            cbxEspecialidad.Name = "cbxEspecialidad";
            cbxEspecialidad.Size = new Size(206, 36);
            cbxEspecialidad.TabIndex = 8;
            // 
            // lblEspecialidad
            // 
            lblEspecialidad.AutoSize = true;
            lblEspecialidad.Location = new Point(16, 242);
            lblEspecialidad.Name = "lblEspecialidad";
            lblEspecialidad.Size = new Size(120, 28);
            lblEspecialidad.TabIndex = 6;
            lblEspecialidad.Text = "Especialidad";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(107, 197);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(240, 33);
            txtEmail.TabIndex = 7;
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(107, 158);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(240, 33);
            txtTelefono.TabIndex = 6;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(39, 202);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(59, 28);
            lblEmail.TabIndex = 5;
            lblEmail.Text = "Email";
            // 
            // lblTelefono
            // 
            lblTelefono.AutoSize = true;
            lblTelefono.Location = new Point(16, 169);
            lblTelefono.Name = "lblTelefono";
            lblTelefono.Size = new Size(86, 28);
            lblTelefono.TabIndex = 4;
            lblTelefono.Text = "Telefono";
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(105, 119);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(240, 33);
            txtApellido.TabIndex = 3;
            // 
            // lblApellido
            // 
            lblApellido.AutoSize = true;
            lblApellido.Location = new Point(13, 122);
            lblApellido.Name = "lblApellido";
            lblApellido.Size = new Size(86, 28);
            lblApellido.TabIndex = 2;
            lblApellido.Text = "Apellido";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(107, 80);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(240, 33);
            txtNombre.TabIndex = 1;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(13, 83);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(85, 28);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "Nombre";
            // 
            // picFoto
            // 
            picFoto.BorderStyle = BorderStyle.FixedSingle;
            picFoto.Image = Properties.Resources.depositphotos_391545206_stock_photo_happy_male_medical_doctor_portrait;
            picFoto.Location = new Point(488, 62);
            picFoto.Name = "picFoto";
            picFoto.Size = new Size(150, 150);
            picFoto.SizeMode = PictureBoxSizeMode.Zoom;
            picFoto.TabIndex = 2;
            picFoto.TabStop = false;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.FromArgb(42, 115, 204);
            btnNuevo.Cursor = Cursors.Hand;
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.Font = new Font("Segoe UI", 9F);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(52, 724);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(100, 35);
            btnNuevo.TabIndex = 5;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(42, 115, 204);
            btnGuardar.Cursor = Cursors.Hand;
            btnGuardar.FlatAppearance.BorderSize = 0;
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.Font = new Font("Segoe UI", 9F);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(182, 724);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(100, 35);
            btnGuardar.TabIndex = 6;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.FromArgb(42, 115, 204);
            btnActualizar.Cursor = Cursors.Hand;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.Font = new Font("Segoe UI", 9F);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(313, 724);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(100, 35);
            btnActualizar.TabIndex = 7;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(42, 115, 204);
            btnEliminar.Cursor = Cursors.Hand;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.Font = new Font("Segoe UI", 9F);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(444, 724);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 35);
            btnEliminar.TabIndex = 8;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // btnSubirFoto
            // 
            btnSubirFoto.BackColor = Color.FromArgb(42, 115, 204);
            btnSubirFoto.FlatAppearance.BorderSize = 0;
            btnSubirFoto.FlatStyle = FlatStyle.Flat;
            btnSubirFoto.ForeColor = Color.White;
            btnSubirFoto.Location = new Point(517, 220);
            btnSubirFoto.Name = "btnSubirFoto";
            btnSubirFoto.Size = new Size(34, 34);
            btnSubirFoto.TabIndex = 9;
            btnSubirFoto.Text = "📁";
            btnSubirFoto.UseVisualStyleBackColor = false;
            // 
            // btnEliminarFoto
            // 
            btnEliminarFoto.BackColor = Color.FromArgb(192, 57, 43);
            btnEliminarFoto.Cursor = Cursors.Hand;
            btnEliminarFoto.FlatAppearance.BorderSize = 0;
            btnEliminarFoto.FlatStyle = FlatStyle.Flat;
            btnEliminarFoto.Font = new Font("Segoe UI", 9F);
            btnEliminarFoto.ForeColor = Color.White;
            btnEliminarFoto.Location = new Point(579, 220);
            btnEliminarFoto.Name = "btnEliminarFoto";
            btnEliminarFoto.Size = new Size(34, 34);
            btnEliminarFoto.TabIndex = 10;
            btnEliminarFoto.Text = "🗑";
            btnEliminarFoto.UseVisualStyleBackColor = false;
            // 
            // dgDoctores
            // 
            dgDoctores.AllowUserToAddRows = false;
            dgDoctores.AllowUserToDeleteRows = false;
            dgDoctores.AllowUserToResizeRows = false;
            dgDoctores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgDoctores.Columns.AddRange(new DataGridViewColumn[] { colId, colApellido, colEspecialidad, colTelefono, colEmail, colNombre });
            dgDoctores.Location = new Point(13, 460);
            dgDoctores.Name = "dgDoctores";
            dgDoctores.ReadOnly = true;
            dgDoctores.RowHeadersWidth = 62;
            dgDoctores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgDoctores.Size = new Size(748, 225);
            dgDoctores.TabIndex = 11;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "Id";
            colId.MinimumWidth = 8;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Visible = false;
            colId.Width = 80;
            // 
            // colApellido
            // 
            colApellido.DataPropertyName = "Apellido";
            colApellido.HeaderText = "Apellido";
            colApellido.MinimumWidth = 8;
            colApellido.Name = "colApellido";
            colApellido.ReadOnly = true;
            colApellido.Width = 140;
            // 
            // colEspecialidad
            // 
            colEspecialidad.DataPropertyName = "Especialidad";
            colEspecialidad.HeaderText = "Especialidad";
            colEspecialidad.MinimumWidth = 8;
            colEspecialidad.Name = "colEspecialidad";
            colEspecialidad.ReadOnly = true;
            colEspecialidad.Width = 120;
            // 
            // colTelefono
            // 
            colTelefono.DataPropertyName = "Telefono";
            colTelefono.HeaderText = "Teléfono";
            colTelefono.MinimumWidth = 8;
            colTelefono.Name = "colTelefono";
            colTelefono.ReadOnly = true;
            colTelefono.Width = 110;
            // 
            // colEmail
            // 
            colEmail.DataPropertyName = "Email";
            colEmail.HeaderText = "Email";
            colEmail.MinimumWidth = 8;
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            colEmail.Width = 180;
            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "Nombre";
            colNombre.HeaderText = "Nombre";
            colNombre.MinimumWidth = 8;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            colNombre.Width = 140;
            // 
            // txtFiltro
            // 
            txtFiltro.Location = new Point(560, 288);
            txtFiltro.Name = "txtFiltro";
            txtFiltro.Size = new Size(200, 33);
            txtFiltro.TabIndex = 0;
            txtFiltro.TextChanged += txtFiltro_TextChanged;
            // 
            // txtEspecialidadFiltro
            // 
            txtEspecialidadFiltro.Location = new Point(560, 335);
            txtEspecialidadFiltro.Name = "txtEspecialidadFiltro";
            txtEspecialidadFiltro.Size = new Size(200, 33);
            txtEspecialidadFiltro.TabIndex = 1;
            txtEspecialidadFiltro.TextChanged += txtEspecialidadFiltro_TextChanged;
            // 
            // lblFiltroNombre
            // 
            lblFiltroNombre.AutoSize = true;
            lblFiltroNombre.Font = new Font("Segoe UI", 9F);
            lblFiltroNombre.Location = new Point(420, 294);
            lblFiltroNombre.Name = "lblFiltroNombre";
            lblFiltroNombre.Size = new Size(134, 25);
            lblFiltroNombre.TabIndex = 12;
            lblFiltroNombre.Text = "Buscar Nombre";
            // 
            // lblFiltroEspecialidad
            // 
            lblFiltroEspecialidad.AutoSize = true;
            lblFiltroEspecialidad.Font = new Font("Segoe UI", 9F);
            lblFiltroEspecialidad.Location = new Point(389, 343);
            lblFiltroEspecialidad.Name = "lblFiltroEspecialidad";
            lblFiltroEspecialidad.Size = new Size(165, 25);
            lblFiltroEspecialidad.TabIndex = 13;
            lblFiltroEspecialidad.Text = "Buscar Especialidad";
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(473, 384);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(112, 34);
            btnBuscar.TabIndex = 14;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // btnMostrarTodos
            // 
            btnMostrarTodos.Location = new Point(637, 384);
            btnMostrarTodos.Name = "btnMostrarTodos";
            btnMostrarTodos.Size = new Size(112, 34);
            btnMostrarTodos.TabIndex = 15;
            btnMostrarTodos.Text = "Mostrar";
            btnMostrarTodos.UseVisualStyleBackColor = true;
            // 
            // DoctoresUI
            // 
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(772, 782);
            Controls.Add(btnMostrarTodos);
            Controls.Add(btnBuscar);
            Controls.Add(lblFiltroEspecialidad);
            Controls.Add(lblFiltroNombre);
            Controls.Add(txtEspecialidadFiltro);
            Controls.Add(txtFiltro);
            Controls.Add(dgDoctores);
            Controls.Add(btnEliminarFoto);
            Controls.Add(btnSubirFoto);
            Controls.Add(btnEliminar);
            Controls.Add(btnActualizar);
            Controls.Add(btnGuardar);
            Controls.Add(btnNuevo);
            Controls.Add(picFoto);
            Controls.Add(gbDoctores);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 9.75F);
            Name = "DoctoresUI";
            Text = " ";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            gbDoctores.ResumeLayout(false);
            gbDoctores.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picFoto).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgDoctores).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private void btnMostrarTodos_Click(object sender, EventArgs e)
        {
            CargarGrid();
            txtFiltro.Clear();
            txtEspecialidadFiltro.Clear();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            CargarGrid(txtFiltro.Text, txtEspecialidadFiltro.Text);
        }

        private void txtEspecialidadFiltro_TextChanged(object sender, EventArgs e)
        {
            CargarGrid(txtFiltro.Text, txtEspecialidadFiltro.Text);
        }
    }
}
