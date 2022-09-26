using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace StudentManager { public partial class MainFrm : Form { private DataAccessLayer dataAccessLayer; private DataTable subjectDataTable; private DataTable studentDataTable; private DataTable teacherDataTable; private BindingSource subjectBindingSource; private BindingSource studentBindingSource; public MainFrm() { InitializeComponent(); dataAccessLayer = new DataAccessLayer(ConfigurationManager.ConnectionStrings["smDB"].ConnectionString); subjectDataTable = getSubjects(); studentDataTable = getStudents(); teacherDataTable = getTeachers(); subjectBindingSource = new BindingSource(); studentBindingSource = new BindingSource(); }
        private DataTable getSubjects()
        {
            DataTable dataTable = dataAccessLayer.getDataTable("SELECT s.ID, s.SubjectCode AS [Subject code], s.SubjectName AS [Subject name], s.Credit AS Credit, " + "s.TypeOfSubjectRequirement AS [Requirement type], t.Name AS Teacher, s.InstituteResponsibleForTheSubject AS Institute " +
                "FROM Subject s " + "JOIN Teacher t ON t.ID = s.TeacherID ", CommandType.Text); return dataTable;
        }
        private DataTable getStudents()
        {
            DataTable dataTable = dataAccessLayer.getDataTable("SELECT st.ID, st.NeptunCode AS [Neptun code], su.SubjectCode AS [Subject code], st.LastName AS [Last name], " + "st.FirstName AS [First name], st.BirthDate AS [Birth Date], st.City, st.Street, st.Gender " + 
                "FROM Student st " + "JOIN Subject su ON su.ID = st.SubjectCode", CommandType.Text);
            return dataTable; 
        }
        private DataTable getTeachers() 
        { 
            DataTable dataTable = dataAccessLayer.getDataTable("SELECT NeptunCode AS [Neptun code], Name, BirthDate AS [Birth date], City, Street, Gender " +
                "FROM Teacher", CommandType.Text); 
            return dataTable;
        }
        private void MainFrm_Load(object sender, EventArgs e)
        {
            btnNewOfSubjectInputGrpBx.Enabled = true;
            btnInsertOfSubjectInputGrpBx.Enabled = false;
            btnNewOfStudentInputGrpBx.Enabled = true; 
            btnInsertOfStudentInputGrpBx.Enabled = false; 
            setDtGrdVwSubject();
            clearDataBindingsSubject();
            setDataBindingsSubject();
            setDtGrdVwStudent(); 
            clearDataBindingsStudent(); 
            setDataBindingsStudent();
            dtGrdVwSubject.Columns["ID"].Visible = false;
            dtGrdVwSubject.CurrentCell = dtGrdVwSubject.Rows[0].Cells[1]; 
            setCmbBxView(cmbBxTeacherOfSubjectInputGrpBx, teacherDataTable, "Name", "Name", dtGrdVwSubject.CurrentRow.Cells["Teacher"].Value);
            dtGrdVwStudent.Columns["ID"].Visible = false;
            dtGrdVwStudent.CurrentCell = dtGrdVwStudent.Rows[0].Cells[1]; 
            setCmbBxView(cmbBxSubjectCodeOfStudentInputGrpBx, subjectDataTable, "Subject code", "Subject code", dtGrdVwStudent.CurrentRow.Cells["Subject code"].Value);
            foreach (DataColumn item in subjectDataTable.Columns)
                if (item.ColumnName != "ID") 
                    cmbBxFilterColumnOfSubjectFilterGrpBx.Items.Add(item.ColumnName);
            foreach (DataColumn item in studentDataTable.Columns) 
                if (item.ColumnName != "ID") 
                    cmbBxFilterColumnOfStudentFilterGrpBx.Items.Add(item.ColumnName); 
            refreshSubjectRecordLabel(); 
            refreshStudentRecordLabel();
        }
        private void setDtGrdVwSubject() 
        {
            subjectBindingSource.DataSource = subjectDataTable; 
            dtGrdVwSubject.DataSource = subjectBindingSource; 
            for (int i = 1; i < dtGrdVwSubject.Columns.Count; i++)
            { 
                dtGrdVwSubject.AutoResizeColumn(i); 
                dtGrdVwSubject.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; 
            }
        }
        private void clearDataBindingsSubject()
        {
            txtBxSubjectCodeOfSubjectInputGrpBx.DataBindings.Clear();
            txtBxSubjectNameOfSubjectInputGrpBx.DataBindings.Clear();
            NumUpDownCreditOfSubjectInputGrpBx.DataBindings.Clear();
            txtBxTypeOfRequirementOfSubjectInputGrpBx.DataBindings.Clear();
            cmbBxTeacherOfSubjectInputGrpBx.DataBindings.Clear();
            txtBxInstituteOfSubjectInputGrpBx.DataBindings.Clear();
        }
        private void setDataBindingsSubject() { txtBxSubjectCodeOfSubjectInputGrpBx.DataBindings.Add(new Binding("Text", subjectBindingSource, "Subject code")); txtBxSubjectNameOfSubjectInputGrpBx.DataBindings.Add(new Binding("Text", subjectBindingSource, "Subject name")); NumUpDownCreditOfSubjectInputGrpBx.DataBindings.Add(new Binding("Value", subjectBindingSource, "Credit")); txtBxTypeOfRequirementOfSubjectInputGrpBx.DataBindings.Add(new Binding("Text", subjectBindingSource, "Requirement type")); cmbBxTeacherOfSubjectInputGrpBx.DataBindings.Add(new Binding("Text", subjectBindingSource, "Teacher")); txtBxInstituteOfSubjectInputGrpBx.DataBindings.Add(new Binding("Text", subjectBindingSource, "Institute")); }
        private void clearDataBindingsStudent() { txtBxNeptunCodeOfStudentInputGrpBx.DataBindings.Clear(); cmbBxSubjectCodeOfStudentInputGrpBx.DataBindings.Clear(); txtBxSurnameOfStudentInputGrpBx.DataBindings.Clear(); txtBxFirstNameOfStudentInputGrpBx.DataBindings.Clear(); mskdTxtBxDateOfBirthOfStudentInputGrpBx.DataBindings.Clear(); txtBxCityOfStudentInputGrpBx.DataBindings.Clear(); txtBxStreetOfStudentInputGrpBx.DataBindings.Clear(); cmbBxGenderOfStudentInputGrpBx.DataBindings.Clear(); }
        private void setDataBindingsStudent()
        {
            txtBxNeptunCodeOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "Neptun code")); cmbBxSubjectCodeOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "Subject code")); txtBxSurnameOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "Last name")); txtBxFirstNameOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "First name")); mskdTxtBxDateOfBirthOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "Birth Date"));
            txtBxCityOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "City")); txtBxStreetOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "Street")); cmbBxGenderOfStudentInputGrpBx.DataBindings.Add(new Binding("Text", studentBindingSource, "Gender"));
        }
        private void setDtGrdVwStudent() { if (dtGrdVwSubject.CurrentRow != null) { studentBindingSource.DataSource = getFilteredDataView(studentDataTable, "[Subject code] = '" + subjectDataTable.Rows[dtGrdVwSubject.CurrentRow.Index][1].ToString() + "'"); dtGrdVwStudent.DataSource = studentBindingSource; for (int i = 0; i < dtGrdVwStudent.Columns.Count; i++) { dtGrdVwStudent.AutoResizeColumn(i); dtGrdVwStudent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; } } else { clearDataBindingsStudent(); studentBindingSource.DataMember = ""; studentBindingSource.DataSource = null; } }
        private DataView getFilteredDataView(DataTable dataTable, string rowFilter) { DataView dataView = new DataView(); dataView = dataTable.DefaultView; dataView.RowFilter = rowFilter; return dataView; }
        private void setCmbBxView(ComboBox comboBox, DataTable dataTable, string displayMember, string valueMember, object selectedValue) { comboBox.DataSource = dataTable; comboBox.DisplayMember = displayMember; comboBox.ValueMember = valueMember; comboBox.SelectedValue = selectedValue; }
        private void refreshSubjectRecordLabel() { txtBxActualRecordOfSubject.Text = ((subjectBindingSource.Position) + 1).ToString(); lblTotalRecordNumberOfSubject.Text = " total " + subjectBindingSource.Count; }
        private void refreshStudentRecordLabel() { txtBxActualRecordOfStudent.Text = ((studentBindingSource.Position) + 1).ToString(); lblTotalRecordNumberOfStudent.Text = " total " + studentBindingSource.Count; }
        private void btnMoveToFirstSubjectRecord_Click(object sender, EventArgs e)

        { subjectBindingSource.MoveFirst(); refreshSubjectRecordLabel(); refreshStudentRecordLabel(); }
        private void btnMoveToPreviousSubjectRecord_Click(object sender, EventArgs e) { subjectBindingSource.MovePrevious(); refreshSubjectRecordLabel(); refreshStudentRecordLabel(); }
        private void btnMoveToNextSubjectRecord_Click(object sender, EventArgs e) { subjectBindingSource.MoveNext(); refreshSubjectRecordLabel(); refreshStudentRecordLabel(); }
        private void btnMoveToLastSubjectRecord_Click(object sender, EventArgs e) { subjectBindingSource.MoveLast(); refreshSubjectRecordLabel(); refreshStudentRecordLabel(); }
        private void txtBxActualRecordOfSubject_TextChanged(object sender, EventArgs e) { if (txtBxActualRecordOfSubject.Text != "") { try { subjectBindingSource.Position = Convert.ToInt32(txtBxActualRecordOfSubject.Text) - 1; dtGrdVwSubject.ClearSelection(); dtGrdVwSubject.Rows[subjectBindingSource.Position].Selected = true; setDtGrdVwStudent(); dtGrdVwSubject.CurrentCell = dtGrdVwSubject.Rows[subjectBindingSource.Position].Cells[1]; } catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } } }
        private void btnNewOfSubjectInputGrpBx_Click(object sender, EventArgs e)
        {
            clearDataBindingsSubject(); txtBxSubjectCodeOfSubjectInputGrpBx.Text = ""; txtBxSubjectNameOfSubjectInputGrpBx.Text = ""; NumUpDownCreditOfSubjectInputGrpBx.Value = 0; txtBxTypeOfRequirementOfSubjectInputGrpBx.Text = ""; cmbBxTeacherOfSubjectInputGrpBx.Text = ""; txtBxInstituteOfSubjectInputGrpBx.Text = ""; btnNewOfSubjectInputGrpBx.Enabled = false; btnInsertOfSubjectInputGrpBx.Enabled = true; btnUpdateOfSubjectInputGrpBx.Enabled = false;

            dtGrdVwSubject.ClearSelection();
        }
        private void btnInsertOfSubjectInputGrpBx_Click(object sender, EventArgs e) { if (!isSubjectTextBoxValuesEmpty()) { Subject subject = getSubject(); List<SqlParameter> parameters = getSqlParameters(subject); int lastId = 0; dataAccessLayer.insert("INSERT INTO Subject (SubjectCode, SubjectName, Credit, TypeOfSubjectRequirement, TeacherID, InstituteResponsibleForTheSubject) " + "OUTPUT INSERTED.ID " + "VALUES (@SubjectCode, @SubjectName, @Credit, @TypeOfSubjectRequirement, @TeacherID, @InstituteResponsibleForTheSubject)", CommandType.Text, parameters.ToArray(), out lastId); subjectDataTable = getSubjects(); subjectBindingSource.DataSource = subjectDataTable; dtGrdVwSubject.DataSource = subjectBindingSource; foreach (DataGridViewRow row in dtGrdVwSubject.Rows) if (row.Cells["ID"].Value.Equals(lastId)) { subjectBindingSource.Position = row.Index; dtGrdVwSubject.Rows[row.Index].Selected = true; } btnNewOfSubjectInputGrpBx.Enabled = true; btnInsertOfSubjectInputGrpBx.Enabled = false; btnUpdateOfSubjectInputGrpBx.Enabled = true; refreshSubjectRecordLabel(); setDataBindingsSubject(); } }
        private bool isSubjectTextBoxValuesEmpty() { if (txtBxSubjectCodeOfSubjectInputGrpBx.Text == "") { errorProvider.SetError(txtBxSubjectCodeOfSubjectInputGrpBx, "Subject code has to be filled!"); return true; } else errorProvider.SetError(txtBxSubjectCodeOfSubjectInputGrpBx, ""); if (txtBxSubjectNameOfSubjectInputGrpBx.Text == "") { errorProvider.SetError(txtBxSubjectNameOfSubjectInputGrpBx, "Subject name has to be filled!"); return true; } else errorProvider.SetError(txtBxSubjectNameOfSubjectInputGrpBx, ""); return false; }
        private object getIDOf(string Field, string FromTable, string WhereField, string WhereValue)
        {
            object scalar = dataAccessLayer.getScalarValue("SELECT " + Field + " FROM " + FromTable + " WHERE " + WhereField + "=" + "'" + WhereValue + "'", CommandType.Text); return scalar;
        }
        private Subject getSubject() { var subject = new Subject { SubjectCode = txtBxSubjectCodeOfSubjectInputGrpBx.Text, SubjectName = txtBxSubjectNameOfSubjectInputGrpBx.Text, Credit = Convert.ToInt16(NumUpDownCreditOfSubjectInputGrpBx.Value), TypeOfSubjectRequirement = txtBxTypeOfRequirementOfSubjectInputGrpBx.Text, TeacherID = getIDOf("ID", "Teacher", "Name", cmbBxTeacherOfSubjectInputGrpBx.SelectedValue.ToString()).ToString(), InstituteResponsibleForTheSubject = txtBxInstituteOfSubjectInputGrpBx.Text }; return subject; }
        private List<SqlParameter> getSqlParameters(Subject subject) { var parameters = new List<SqlParameter>(); parameters.Add(dataAccessLayer.createParameter("@SubjectCode", 12, subject.SubjectCode, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@SubjectName", 50, subject.SubjectName, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@Credit", subject.Credit, DbType.Int32)); parameters.Add(dataAccessLayer.createParameter("@TypeOfSubjectRequirement", 30, subject.TypeOfSubjectRequirement, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@TeacherID", 6, subject.TeacherID, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@InstituteResponsibleForTheSubject", 50, subject.InstituteResponsibleForTheSubject, DbType.String)); return parameters; }
        private void dtGrdVwSubject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure? After deleting no any undo!", "Deletion confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question); if (dialogResult == DialogResult.Yes)
                {
                    int numberOfDeletedRow = 0; for (int i = 0; i < dtGrdVwSubject.SelectedRows.Count; i++)
                    {
                        var subject = new Subject { ID = (int)dtGrdVwSubject.SelectedRows[i].Cells["ID"].Value }; var parameters = new List<SqlParameter>();


                        parameters.Add(dataAccessLayer.createParameter("@ID", subject.ID, DbType.Int32)); dataAccessLayer.delete("DELETE FROM Subject WHERE ID = @ID", CommandType.Text, parameters.ToArray()); numberOfDeletedRow++;
                    }
                    subjectDataTable = getSubjects(); subjectBindingSource.DataSource = subjectDataTable; dtGrdVwSubject.DataSource = subjectBindingSource; clearDataBindingsSubject(); setDataBindingsSubject(); refreshSubjectRecordLabel(); MessageBox.Show(numberOfDeletedRow.ToString() + " row(s) has been deleted.", "Deletion result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void dtGrdVwSubject_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Up) { refreshSubjectRecordLabel(); } else if (e.KeyCode == Keys.Down) { refreshSubjectRecordLabel(); } else if (e.KeyCode == Keys.PageUp) { refreshSubjectRecordLabel(); } else if (e.KeyCode == Keys.PageDown) { refreshSubjectRecordLabel(); } }
        private void dtGrdVwSubject_CellClick(object sender, DataGridViewCellEventArgs e) { refreshSubjectRecordLabel(); refreshStudentRecordLabel(); }
        private void btnUpdateOfSubjectInputGrpBx_Click(object sender, EventArgs e)
        {
            if (!isSubjectTextBoxValuesEmpty())
            {
                Subject subject = getSubject(); subject.ID = (int)dtGrdVwSubject.CurrentRow.Cells["ID"].Value; List<SqlParameter> parameters = getSqlParameters(subject); parameters.Add(dataAccessLayer.createParameter("@ID", subject.ID, DbType.Int32)); dataAccessLayer.update("UPDATE Subject SET SubjectCode = @SubjectCode, SubjectName = @SubjectName, Credit = @Credit, TypeOfSubjectRequirement = " + "@TypeOfSubjectRequirement, TeacherID = @TeacherID, InstituteResponsibleForTheSubject = @InstituteResponsibleForTheSubject WHERE ID=@ID", CommandType.Text, parameters.ToArray()); subjectDataTable = getSubjects(); subjectBindingSource.DataSource = subjectDataTable;

                dtGrdVwSubject.DataSource = subjectBindingSource; studentDataTable = getStudents(); studentBindingSource.DataSource = studentDataTable; dtGrdVwStudent.DataSource = studentBindingSource; clearDataBindingsStudent(); setDataBindingsStudent(); refreshStudentRecordLabel(); setDtGrdVwStudent();
            }
        }
        private void txtBxSubjectCodeOfSubjectInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfSubjectCodeOfSubjectInputGrpBx.Text = "(" + txtBxSubjectCodeOfSubjectInputGrpBx.Text.Length + "/12)"; }
        private void txtBxSubjectNameOfSubjectInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfSubjectNameOfSubjectInputGrpBx.Text = "(" + txtBxSubjectNameOfSubjectInputGrpBx.Text.Length + "/50)"; }
        private void txtBxTypeOfRequirementOfSubjectInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfRequirementOfSubjectInputGrpBx.Text = "(" + txtBxTypeOfRequirementOfSubjectInputGrpBx.Text.Length + "/30)"; }
        private void txtBxInstituteOfSubjectInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfInstituteOfSubjectInputGrpBx.Text = "(" + txtBxInstituteOfSubjectInputGrpBx.Text.Length + "/50)"; }
        private void txtBxPatternOfSubjectFilterGrpBx_TextChanged(object sender, EventArgs e) { Dictionary<string, string> fieldNames = new Dictionary<string, string>() { { "Subject code", "SubjectCode" }, { "Subject name", "SubjectName" }, { "Credit", "Credit" }, { "TypeOfSubjectRequirement", "Requirement type" }, { "Name", "Teacher" }, { "InstituteResponsibleForTheSubject", "Institute" }, }; subjectDataTable = dataAccessLayer.getDataTable("SELECT s.ID, s.SubjectCode AS [Subject code], s.SubjectName AS [Subject name], s.Credit AS Credit, " + "s.TypeOfSubjectRequirement AS [Requirement type], t.Name AS Teacher, s.InstituteResponsibleForTheSubject AS Institute " + "FROM Subject s " + "JOIN Teacher t ON t.ID = s.TeacherID " + "WHERE [" + fieldNames[cmbBxFilterColumnOfSubjectFilterGrpBx.SelectedItem.ToString()] + "] LIKE '%" + txtBxPatternOfSubjectFilterGrpBx.Text + "%'", CommandType.Text);
            subjectBindingSource.DataSource = subjectDataTable; dtGrdVwSubject.DataSource = subjectBindingSource; setDtGrdVwSubject(); clearDataBindingsSubject(); setDataBindingsSubject(); setDtGrdVwStudent(); refreshSubjectRecordLabel(); refreshStudentRecordLabel();
        }
        private void btnMoveToFirstStudentRecord_Click(object sender, EventArgs e) { studentBindingSource.MoveFirst(); refreshStudentRecordLabel(); }
        private void btnMoveToPreviousStudentRecord_Click(object sender, EventArgs e) { studentBindingSource.MovePrevious(); refreshStudentRecordLabel(); }
        private void btnMoveToNextStudentRecord_Click(object sender, EventArgs e) { studentBindingSource.MoveNext(); refreshStudentRecordLabel(); }
        private void btnMoveToLastStudentRecord_Click(object sender, EventArgs e) { studentBindingSource.MoveLast(); refreshStudentRecordLabel(); }
        private void txtBxActualRecordOfStudent_TextChanged(object sender, EventArgs e) { if (txtBxActualRecordOfStudent.Text != "") { try { studentBindingSource.Position = Convert.ToInt32(txtBxActualRecordOfStudent.Text) - 1; dtGrdVwStudent.ClearSelection(); dtGrdVwStudent.Rows[studentBindingSource.Position].Selected = true; dtGrdVwStudent.CurrentCell = dtGrdVwStudent.Rows[studentBindingSource.Position].Cells[1]; } catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } } }
        private void btnNewOfStudentInputGrpBx_Click(object sender, EventArgs e) { clearDataBindingsStudent(); txtBxNeptunCodeOfStudentInputGrpBx.Text = ""; txtBxSurnameOfStudentInputGrpBx.Text = ""; txtBxFirstNameOfStudentInputGrpBx.Text = ""; mskdTxtBxDateOfBirthOfStudentInputGrpBx.Text = "";
            txtBxCityOfStudentInputGrpBx.Text = ""; txtBxStreetOfStudentInputGrpBx.Text = ""; cmbBxGenderOfStudentInputGrpBx.Text = ""; btnNewOfStudentInputGrpBx.Enabled = false; btnInsertOfStudentInputGrpBx.Enabled = true; btnUpdateOfStudentInputGrpBx.Enabled = false; dtGrdVwStudent.ClearSelection();
        }
        private void btnInsertOfStudentInputGrpBx_Click(object sender, EventArgs e) { if (!isStudentTextBoxValuesEmpty()) { Student student = getStudent(); List<SqlParameter> parameters = getSqlParameters(student); int lastId = 0; dataAccessLayer.insert("INSERT INTO Student " + "OUTPUT INSERTED.ID " + "VALUES (@NeptunCode, @SubjectCode, @LastName, @FirstName, @BirthDate, @City, @Street, @Gender)", CommandType.Text, parameters.ToArray(), out lastId); studentDataTable = getStudents(); studentBindingSource.DataSource = studentDataTable; dtGrdVwStudent.DataSource = studentBindingSource; foreach (DataGridViewRow row in dtGrdVwStudent.Rows) if (row.Cells["ID"].Value.Equals(lastId)) { studentBindingSource.Position = row.Index; dtGrdVwStudent.Rows[row.Index].Selected = true; } btnNewOfStudentInputGrpBx.Enabled = true; btnInsertOfSubjectInputGrpBx.Enabled = false; btnUpdateOfStudentInputGrpBx.Enabled = true; setDataBindingsStudent(); setDtGrdVwStudent(); refreshStudentRecordLabel(); } }
        private bool isStudentTextBoxValuesEmpty() { if (txtBxNeptunCodeOfStudentInputGrpBx.Text == "") { errorProvider.SetError(txtBxNeptunCodeOfStudentInputGrpBx, "Neptun code has to be filled!"); return true; } else errorProvider.SetError(txtBxNeptunCodeOfStudentInputGrpBx, ""); if (txtBxSurnameOfStudentInputGrpBx.Text == "") { errorProvider.SetError(txtBxSurnameOfStudentInputGrpBx, "Surname has to be filled!"); return true; } else

                errorProvider.SetError(txtBxSurnameOfStudentInputGrpBx, ""); if (txtBxFirstNameOfStudentInputGrpBx.Text == "") { errorProvider.SetError(txtBxFirstNameOfStudentInputGrpBx, "First name has to be filled!"); return true; } else errorProvider.SetError(txtBxFirstNameOfStudentInputGrpBx, ""); if (!mskdTxtBxDateOfBirthOfStudentInputGrpBx.MaskCompleted) { errorProvider.SetError(mskdTxtBxDateOfBirthOfStudentInputGrpBx, "Date has to be filled!"); return true; } else errorProvider.SetError(mskdTxtBxDateOfBirthOfStudentInputGrpBx, ""); if (txtBxCityOfStudentInputGrpBx.Text == "") { errorProvider.SetError(txtBxCityOfStudentInputGrpBx, "City has to be filled!"); return true; } else errorProvider.SetError(txtBxCityOfStudentInputGrpBx, ""); if (txtBxStreetOfStudentInputGrpBx.Text == "") { errorProvider.SetError(txtBxStreetOfStudentInputGrpBx, "Address has to be filled!"); return true; } else errorProvider.SetError(txtBxStreetOfStudentInputGrpBx, ""); return false;
        }
        private void mskdTxtBxDateOfBirthOfStudentInputGrpBx_TypeValidationCompleted(object sender, TypeValidationEventArgs e) { if (!e.IsValidInput) { errorProvider.SetError(mskdTxtBxDateOfBirthOfStudentInputGrpBx, "Use valid date!"); } else errorProvider.SetError(mskdTxtBxDateOfBirthOfStudentInputGrpBx, ""); }
        private Student getStudent() { var student = new Student { NeptunCode = txtBxNeptunCodeOfStudentInputGrpBx.Text, SubjectCode = getIDOf("ID", "Subject", "SubjectCode", cmbBxSubjectCodeOfStudentInputGrpBx.SelectedValue.ToString()).ToString(), LastName = txtBxSurnameOfStudentInputGrpBx.Text,

            FirstName = txtBxFirstNameOfStudentInputGrpBx.Text,
            BirthDate = DateTime.Parse(mskdTxtBxDateOfBirthOfStudentInputGrpBx.Text),
            City = txtBxCityOfStudentInputGrpBx.Text,
            Street = txtBxStreetOfStudentInputGrpBx.Text,
            Gender = cmbBxGenderOfStudentInputGrpBx.Text
        }; return student;
        }
        private List<SqlParameter> getSqlParameters(Student student) { var parameters = new List<SqlParameter>(); parameters.Add(dataAccessLayer.createParameter("@NeptunCode", 6, student.NeptunCode, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@SubjectCode", 12, student.SubjectCode, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@LastName", 20, student.LastName, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@FirstName", 20, student.FirstName, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@BirthDate", student.BirthDate, DbType.DateTime)); parameters.Add(dataAccessLayer.createParameter("@City", 20, student.City, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@Street", 80, student.Street, DbType.String)); parameters.Add(dataAccessLayer.createParameter("@Gender", 6, student.Gender, DbType.String)); return parameters; }
        private void dtGrdVwStudent_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Delete) { DialogResult dialogResult = MessageBox.Show("Are you sure? After deleting no any undo!", "Deletion confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question); if (dialogResult == DialogResult.Yes) { int numberOfDeletedRow = 0; for (int i = 0; i < dtGrdVwStudent.SelectedRows.Count; i++) { var student = new Student { ID = (int)dtGrdVwStudent.SelectedRows[i].Cells["ID"].Value }; var parameters = new List<SqlParameter>(); parameters.Add(dataAccessLayer.createParameter("@ID", student.ID, DbType.Int32)); dataAccessLayer.delete("DELETE FROM Student WHERE ID = @ID", CommandType.Text, parameters.ToArray()); numberOfDeletedRow++; } studentDataTable = getStudents(); studentBindingSource.DataSource = studentDataTable; dtGrdVwStudent.DataSource = studentBindingSource; clearDataBindingsStudent(); setDataBindingsStudent(); setDtGrdVwStudent();

                    refreshStudentRecordLabel(); MessageBox.Show(numberOfDeletedRow.ToString() + " row(s) has been deleted.", "Deletion result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void dtGrdVwStudent_KeyUp(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Up) { refreshStudentRecordLabel(); } else if (e.KeyCode == Keys.Down) { refreshStudentRecordLabel(); } else if (e.KeyCode == Keys.PageUp) { refreshStudentRecordLabel(); } else if (e.KeyCode == Keys.PageDown) { refreshStudentRecordLabel(); } }
        private void dtGrdVwStudent_CellClick(object sender, DataGridViewCellEventArgs e) { refreshStudentRecordLabel(); }
        private void btnUpdateOfStudentInputGrpBx_Click(object sender, EventArgs e) { if (!isStudentTextBoxValuesEmpty()) { Student student = getStudent(); student.ID = (int)dtGrdVwStudent.CurrentRow.Cells["ID"].Value; List<SqlParameter> parameters = getSqlParameters(student); parameters.Add(dataAccessLayer.createParameter("@ID", student.ID, DbType.Int32)); dataAccessLayer.update("UPDATE Student SET NeptunCode = @NeptunCode, SubjectCode = @SubjectCode, LastName = @LastName, FirstName = @FirstName, BirthDate = @BirthDate, " + "City = @City, Street = @Street, Gender = @Gender WHERE ID = @ID", CommandType.Text, parameters.ToArray()); studentDataTable = getStudents(); studentBindingSource.DataSource = studentDataTable; dtGrdVwStudent.DataSource = studentBindingSource; refreshStudentRecordLabel(); setDtGrdVwStudent(); } }
        private void txtBxNeptunCodeOfStudentInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfNeptunCodeOfStudentInputGrpBx.Text = "(" + txtBxNeptunCodeOfStudentInputGrpBx.Text.Length + "/6)"; }

        private void txtBxSurnameOfStudentInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfSurnameOfStudentInputGrpBx.Text = "(" + txtBxSurnameOfStudentInputGrpBx.Text.Length + "/20)"; }
        private void txtBxFirstNameOfStudentInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfFirstNameOfStudentInputGrpBx.Text = "(" + txtBxFirstNameOfStudentInputGrpBx.Text.Length + "/20)"; }
        private void txtBxCityOfStudentInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfCityOfStudentInputGrpBx.Text = "(" + txtBxCityOfStudentInputGrpBx.Text.Length + "/20)"; }
        private void txtBxStreetOfStudentInputGrpBx_TextChanged(object sender, EventArgs e) { lblMaxLengthOfStreetOfStudentInputGrpBx.Text = "(" + txtBxStreetOfStudentInputGrpBx.Text.Length + "/80)"; }
        private void txtBxPatternOfStudentFilterGrpBx_TextChanged(object sender, EventArgs e) { Dictionary<string, string> fieldNames = new Dictionary<string, string>() { { "Neptun code", "NeptunCode" }, { "Subject code", "SubjectCode" }, { "Last name", "LastName" }, { "First name", "FirstName" }, { "Birth date", "BirthDate" }, { "City", "City" }, { "Street", "Street" }, { "Gender", "Gender" } }; studentDataTable = dataAccessLayer.getDataTable("SELECT st.ID, st.NeptunCode AS [Neptun code], su.SubjectCode AS [Subject code], st.LastName AS [Last name], " + "st.FirstName AS [First name], st.BirthDate AS [Birth Date], st.City, st.Street, st.Gender " + "FROM Student st " + "JOIN Subject su ON su.ID = st.SubjectCode " + "WHERE [" + fieldNames[cmbBxFilterColumnOfStudentFilterGrpBx.SelectedItem.ToString()] + "] LIKE '%" + txtBxPatternOfStudentFilterGrpBx.Text + "%'", CommandType.Text); studentBindingSource.DataSource = studentDataTable; dtGrdVwStudent.DataSource = studentBindingSource; clearDataBindingsStudent(); setDataBindingsStudent(); setDtGrdVwStudent(); refreshStudentRecordLabel(); }
    }
}