using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using Svg;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using INTUSOFT.Data.Repository;
using INTUSOFT.Data;
using INTUSOFT.Data.Model;
using INTUSOFT.Data.Enumdetails;
using INTUSOFT.Data.NewDbModel;
using INTUSOFT.Data.Mapping;
using MySql.Data;
using System.Collections;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;
using NLog.Targets;

namespace DBPorting
{
    public partial class DbTransferForm : Form
    {
        Pagination portingForm;
        TimeProfiling timeProfiling;
        List<INTUSOFT.Data.Model.Patient> pats = null;
        List<VisitModel> visits = null;
        users user;
        patient_identifier_type patientidetifiertype;
        Concept concept;
        machine machine1;
        List<ImageModel> image = null;
        SvgDocument svgDoc;
        List<Report> reports = null;
        List<AnnotationModel> annotation = null;
        XmlSerializer maskSettingsSerializer;
        XmlSerializer captureSettingsSerializer;
        string dbFile = string.Empty;
        string dateTime = "0001-01-01 00-00-00";
        DateTime tempdt = new DateTime();
        char maleStr = 'M';
        char femaleStr = 'F';
        char leftSideChar = 'L';
        char rightSideChar = 'R';
        string ConfigFileName = "IVLConfig.xml";
        INTUSOFT.Data.NewDbModel.Patient newPatient;
        INTUSOFT.Data.NewDbModel.PersonAddressModel personAddress;
        INTUSOFT.Data.NewDbModel.patient_identifier patientIdentifier;
        INTUSOFT.Data.NewDbModel.visit newVisit;
        INTUSOFT.Data.NewDbModel.obs newObs;
        INTUSOFT.Data.NewDbModel.eye_fundus_image newEye;
        INTUSOFT.Data.NewDbModel.report newreport;
        ReportType reportType;
        string captureSettingsValue;
        string maskSettingsValue;
        eye_fundus_image newImage;
        INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation newAnnotation;
        public Logger Exception_Log = LogManager.GetLogger("DB_Porting.ExceptionLog");

        public DbTransferForm()
        {
            //AdminAdditionOperation.Forms.Admin_UC.createPassword("superuser");
            InitializeComponent();
            string appLogoFilePath = @"ImageResources\LogoImageResources\IntuSoft.ico";

            if (File.Exists(appLogoFilePath))
                this.Icon = new System.Drawing.Icon(appLogoFilePath);
            tempdt = DateTime.ParseExact(dateTime, "yyyy-MM-dd hh-mm-ss", System.Globalization.CultureInfo.InvariantCulture);
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines("Intusoft-runtime.properties"))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            NHibernateHelper_MySQL.dbName = data["connection.DBname"];
            NHibernateHelper_MySQL.userName = data["connection.username"];
            NHibernateHelper_MySQL.password = data["connection.password"];
            insertIntoNewDb_btn.Enabled = false;
            NewDataVariables._Repo = Repository.GetInstance();
            //NewDataVariables._patientRepo = NewPatientRepository.GetInstance();
            //NewDataVariables._imageRepo = ObsRepository.GetInstance();
            //NewDataVariables._reportRepo = NewReportRepository.GetInstance();
            //NewDataVariables._annotationRepo = NewAnnotationRepository.GetInstance();
            //NewDataVariables._patIdentfier = PatientIdentifierRepository.GetInstance();
            NewDataVariables.Patients = NewDataVariables._Repo.GetAll<INTUSOFT.Data.NewDbModel.Patient>().ToList();// as List<NewPatient>;
            //NewDataVariables._personaddressRepo = PersonAddressRepository.GetInstance();
            //NewDataVariables._patIdentfier = PatientIdentifierRepository.GetInstance();
            //NewDataVariables._personattributeRepo = PersonAttributeRepository.GetInstance();
            //NewDataVariables._eyeimagerepo = EyeFundusImageRepository.GetInstance();
            //NewDataVariables._obsAttributeRepo = ObservationAttributeRepository.GetInstance();
            newImage = eye_fundus_image.CreateNewEyeFundusImage();
            user = users.CreateNewUsers();
            user.userId = 1;
            patientidetifiertype = patient_identifier_type.CreateNewPatientIdentifierType();
            concept = Concept.CreateNewConcept();
            concept.conceptId = 3;
            machine1 = machine.CreateNewMachine();
            machine1.machineId = 1;
            reportType = ReportType.CreateNewReportType();
            reportType.reportTypeId = 1;
            //portingForm = new Pagination();
            //timeProfiling = new TimeProfiling();
            maskSettingsSerializer = new XmlSerializer(typeof(MaskSettings));
            captureSettingsSerializer = new XmlSerializer(typeof(CaptureLog));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DBtransferForm dbt = DBtransferForm.getInstance();
            //dbt.ShowDialog();
        }

        /// <summary>
        /// Patient details will be added to mysql database from SQLite database.
        /// </summary>
        public void InsertPatient()
        {
            try
            {
                if (IVLVariables._ivlConfig == null)
                    IVLVariables._ivlConfig = IVLConfig.getInstance();
                int StepCount = (int)100 / pats.Count;//Calculates the StepCount for no of patients in pats.
                int count = 1;
                int patsNumber = 1;
                this.Cursor = Cursors.WaitCursor;
                {
                    patientidetifiertype.patientIdentifierTypeId = 1;
                    foreach (INTUSOFT.Data.Model.Patient item in pats)//Add each patient and its details into the Mysql database
                    {
                        InsertPatient_lbl.Text = "Inserting patient " + patsNumber;
                        InsertPatient_lbl.Refresh();
                        newPatient = INTUSOFT.Data.NewDbModel.Patient.CreateNewPatient();
                        personAddress = INTUSOFT.Data.NewDbModel.PersonAddressModel.CreateNewPersonAddress();
                        patientIdentifier = INTUSOFT.Data.NewDbModel.patient_identifier.CreateNewPatientIdentifier();
                        if (item.Gender.Equals(GenderEnum.Male.ToString()))
                            newPatient.gender = maleStr;
                        else
                            newPatient.gender = femaleStr;
                        newPatient.createdBy = user;
                        newPatient.patientCreatedBy = user;
                        newPatient.primaryEmailId = item.Email;
                        newPatient.primaryPhoneNumber = item.Mobile.ToString();
                        newPatient.historyAilments = item.HistoryAilments;
                        newPatient.createdBy = user;
                        if (item.RegistrationDateTime.Year != 1)
                        {
                            newPatient.createdDate = item.RegistrationDateTime;
                            newPatient.patientCreatedDate = item.RegistrationDateTime;
                        }
                        if (item.Modified_DateTime.Year != 1)
                        {
                            newPatient.lastModifiedDate = item.Modified_DateTime;
                            newPatient.patientLastModifiedDate = item.Modified_DateTime;
                        }
                        newPatient.voided = item.HideShowRow;
                        newPatient.birthdateEstimated = false;
                        newPatient.firstName = item.FirstName;
                        newPatient.lastName = item.LastName;
                        if (item.Touched_DateTime.Year != 1)
                        {
                            newPatient.date_accessed = item.Touched_DateTime;
                        }
                        newPatient.birthdate = item.DOB;
                        personAddress.person = newPatient;
                        //Adding address details of the Patient into personAddress.
                        personAddress.preffered = true;
                        personAddress.line1 = item.Address_Local.Address;
                        personAddress.cityVillage = item.Address_Local.City;
                        personAddress.stateProvince = item.Address_Local.State;
                        personAddress.country = item.Address_Local.Country;
                        if (item.Address_Local.Pincode.ToString().Length >= 10)
                            personAddress.postalCode = item.Address_Local.Pincode.ToString().Remove(9);
                        else
                            personAddress.postalCode = item.Address_Local.Pincode.ToString();
                        personAddress.Land_Code = item.Land_Code;
                        if (item.RegistrationDateTime.Year != 1)
                            personAddress.createdDate = item.RegistrationDateTime;
                        if (item.Modified_DateTime.Year != 1)
                            personAddress.lastModifiedDate = item.Modified_DateTime;
                        if (item.Touched_DateTime.Year != 1)
                            personAddress.date_accessed = item.Touched_DateTime;
                        personAddress.voided = item.HideShowRow;
                        personAddress.createdBy = user;
                        patientIdentifier.createdBy = user;

                        //Adding details of patient MRN into patientIdentifier.
                        patientIdentifier.patient = newPatient;
                        patientIdentifier.value = item.MRN;
                        patientIdentifier.type = patientidetifiertype;
                        patientIdentifier.voided = item.HideShowRow;
                        //string history_alignments = item.HistoryAilments.Replace("'", "");
                        if (item.RegistrationDateTime.Year != 1)
                            patientIdentifier.createdDate = item.RegistrationDateTime;
                        if (item.Modified_DateTime.Year != 1)
                            patientIdentifier.lastModifiedDate = item.Modified_DateTime;
                        patientIdentifier.preferred = true;

                        NewDataVariables._Repo.Add<INTUSOFT.Data.NewDbModel.Patient>(newPatient);//Inserts patient deatils into the MySql database.
                        NewDataVariables._Repo.Add<PersonAddressModel>(personAddress);//Inserts address deatils of patient into the MySql database.
                        NewDataVariables._Repo.Add<patient_identifier>(patientIdentifier);//Inserts MRN deatils of patient into the MySql database.

                        PersonAttributes attributes = new PersonAttributes();
                        attributes.Comments = item.Comments;
                        attributes.Height = item.Height.ToString();
                        attributes.Income = item.Income;
                        attributes.Landline = item.Land_Code + " " + item.Landline.ToString();
                        attributes.Occupation = item.Occupation;
                        attributes.Weight = item.Weight.ToString();

                        InsertPatientAttribute(attributes);//Invokes the InsertPatientAttribute method to add the patient attributes into Mysql database.
                        InsertVisit(item.ID);//Invokes the InsertVisit method to add the visit into Mysql database.
                        newPatient.Dispose();
                        personAddress.Dispose();
                        patientIdentifier.Dispose();

                        patsNumber++;
                        count += StepCount;//Calculates the count value by adding the step count.
                        Patient_pgbar.Value = count;
                        PatientNo_lbl.Text = count.ToString() + "%";//Assigns the count value to the PatientNo_lbl.Text
                        PatientNo_lbl.Refresh();
                    }
                }
                Patient_pgbar.Value = 100;
                PatientNo_lbl.Text = Patient_pgbar.Value.ToString() + "%";
                PatientNo_lbl.Refresh();
                this.Cursor = Cursors.Default;
                insertIntoNewDb_btn.Enabled = false;
                patsNumber -= 1;
                MessageBox.Show("Patient Data insertion completed .Number of patients inserted = " + patsNumber.ToString());
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Patient attrubute will be added to the mysql database.
        /// </summary>
        /// <param name="newPatient">newPatient data</param>
        /// <param name="oldPatient">oldPatient data</param>
        public void InsertPatientAttribute(PersonAttributes attributes)
        {
            try
            {
                person_attribute_type personAttributetype = person_attribute_type.CreateNewPersonAttributeType();
                INTUSOFT.Data.NewDbModel.person_attribute personAttribute = INTUSOFT.Data.NewDbModel.person_attribute.CreateNewPersonAttribute();
                personAttribute.person = newPatient;
                personAttribute.value = attributes.Income.ToString();
                personAttributetype.personAttributeTypeId = (int)PatientAttributesType.Income;
                personAttribute.attributeType = personAttributetype;
                NewDataVariables._Repo.Add<person_attribute>(personAttribute);//Insert the attribute Income into the database.
                personAttribute.Dispose();

                personAttribute = new person_attribute();
                personAttribute.person = newPatient;
                personAttribute.value = attributes.Occupation.ToString();
                personAttributetype.personAttributeTypeId = (int)PatientAttributesType.Occupation;
                personAttribute.attributeType = personAttributetype;
                NewDataVariables._Repo.Add<person_attribute>(personAttribute);//Insert the attribute Occupation into the database.
                personAttribute.Dispose();

                personAttribute = new person_attribute();
                personAttribute.person = newPatient;
                personAttribute.value = attributes.Landline;
                personAttributetype.personAttributeTypeId = (int)PatientAttributesType.Landline;
                personAttribute.attributeType = personAttributetype;
                NewDataVariables._Repo.Add<person_attribute>(personAttribute);//Insert the attribute Landline into the database.
                personAttribute.Dispose();

                personAttribute = new person_attribute();
                personAttribute.person = newPatient;
                personAttribute.value = attributes.Height.ToString();
                personAttributetype.personAttributeTypeId = (int)PatientAttributesType.Height;
                personAttribute.attributeType = personAttributetype;
                NewDataVariables._Repo.Add<person_attribute>(personAttribute);//Insert the attribute Height into the database.
                personAttribute.Dispose();

                personAttribute = new person_attribute();
                personAttribute.person = newPatient;
                personAttribute.value = attributes.Weight.ToString();
                personAttributetype.personAttributeTypeId = (int)PatientAttributesType.Weight;
                personAttribute.attributeType = personAttributetype;
                NewDataVariables._Repo.Add<person_attribute>(personAttribute);//Insert the attribute Weight into the database.
                personAttribute.Dispose();

                personAttribute = new person_attribute();
                personAttribute.person = newPatient;
                personAttribute.value = attributes.Comments.ToString();
                personAttributetype.personAttributeTypeId = (int)PatientAttributesType.Comments;
                personAttribute.attributeType = personAttributetype;
                NewDataVariables._Repo.Add<person_attribute>(personAttribute);//Insert the attribute Comments into the database.
                personAttribute.Dispose();
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Will insert visit from SQLite to Mysql.
        /// </summary>
        /// <param name="newPatient">Patient data of type INTUSOFT.Data.NewDbModel.Patient</param>
        /// <param name="oldPatient">Patient data of type INTUSOFT.Data.Model.Patient</param>
        public void InsertVisit(int oldPatientId)
        {
            try
            {
                int count = 1;
                List<VisitModel> oldVisits = visits.Where(x => x.PatientID == oldPatientId).ToList<VisitModel>();
                int StepCount = 0;
                {
                    if (oldVisits.Count > 0)
                        StepCount = (int)100 / oldVisits.Count;//Calculates the StepCount for no of visits in oldVisits.
                    foreach (VisitModel item in oldVisits)//Adds the visits in list oldVisits into the Mysql database.
                    {
                        InsertVisit_lbl.Text = "Inserting visit " + count;
                        InsertVisit_lbl.Refresh();
                        newVisit = INTUSOFT.Data.NewDbModel.visit.CreateNewVisit();
                        newVisit.patient = newPatient;
                        newVisit.createdBy = user;
                        if (item.VisitDateTime.Year != 1)
                            newVisit.createdDate = item.VisitDateTime;
                        if (item.VisitModifyDateTime.Year != 1)
                            newVisit.lastModifiedDate = item.VisitModifyDateTime;
                        if (item.VisitTouchDateTime.Year != 1)
                            newVisit.lastAccessedDate = item.VisitTouchDateTime;
                        newVisit.voided = item.HideShowRow;

                        NewDataVariables._Repo.Add<visit>(newVisit);//Inserts visit data into Mysql server.
                        InsertObservations(item.ID);//Add the images to the Mysql database.
                        InsertReport(item.ID);//Add the reports to the Mysql database.
                        newVisit.Dispose();//Disposes the visit model.
                        count++;

                        if (Visit_pgbar.Value >= 100)//Checks weather the Visit_pgbar.Value is equal to 100 for visits of next patient.
                            Visit_pgbar.Value = 0;
                        Visit_pgbar.Value += StepCount;//Calculates the new Visit_pgbar.Value
                        NoOfVisit_lbl.Text = Visit_pgbar.Value.ToString() + "%";//Assigns the new Visit_pgbar value to the NoOfVisit_lbl.Text.
                        NoOfVisit_lbl.Refresh();
                    }
                    Visit_pgbar.Value = 100;
                    NoOfVisit_lbl.Text = Visit_pgbar.Value.ToString() + "%";//Assigns the new Visit_pgbar 100 value to the NoOfVisit_lbl.Text.
                    NoOfVisit_lbl.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Insert image data from Sqlite to Mysql
        /// </summary>
        /// <param name="newVisit">NewVisit data of type INTUSOFT.Data.NewDbModel.visit</param>
        /// <param name="oldVisit">OldVisit data of type INTUSOFT.Data.Model.VisitModel oldVisit</param>
        public void InsertObservations(int oldVisitId)
        {
            try
            {
                int count = 1;
                List<ImageModel> oldImage = image.Where(x => x.VisitID == oldVisitId).ToList<ImageModel>();
                int StepCount = 0;
                {
                    if (oldImage.Count > 0)
                        StepCount = (int)100 / oldImage.Count;
                    foreach (INTUSOFT.Data.Model.ImageModel item in oldImage)//Adds the images in list oldImage into the Mysql database.
                    {
                        InsertObs_lbl.Text = "Inserting image " + count;
                        InsertObs_lbl.Refresh();
                        newObs = INTUSOFT.Data.NewDbModel.obs.CreateNewObs();
                        newEye = INTUSOFT.Data.NewDbModel.eye_fundus_image.CreateNewEyeFundusImage();
                        newObs.concept = concept;
                        newObs.visit = newVisit;
                        newObs.patient = newVisit.patient;
                        newObs.voided = item.HideShowRow;
                        if (item.ImageDateTime.Year != 1)
                            newObs.createdDate = item.ImageDateTime;
                        if (item.ImageModifyDateTime.Year != 1)
                            newObs.lastModifiedDate = item.ImageModifyDateTime;
                        if (item.ImageTouchDateTime.Year != 1)
                            newObs.takenDateTime = item.ImageDateTime;
                        if (item.ImageTouchDateTime.Year != 1)
                            newEye.lastAccessedDate = item.ImageTouchDateTime;
                        SaveCaptureLog();
                        newEye.cameraSetting = captureSettingsValue;
                        newEye.maskSetting = maskSettingsValue;
                        string[] imagePath = item.LocalURL.Split('\\');//Splits the URL of the image to get the relative path of the image.
                        string imageName = imagePath[imagePath.Length - 1];
                        newObs.createdBy = user;
                        newObs.value = imageName;
                        if (item.EyeSide == 1)//Checks weather the eyeside is leftside or rightside
                            newEye.eyeSide = leftSideChar;
                        else if (item.EyeSide == 0)
                            newEye.eyeSide = rightSideChar;
                        newEye.machine = machine1;
                        NewDataVariables._Repo.Add<obs>(newObs);//Inserts the image details into the Mysql database.
                        newEye.eye_fundus_image_id = newObs.observationId;
                        NewDataVariables._Repo.Add<eye_fundus_image>(newEye);//Inserts the image details into the Mysql database.
                        //Adds annotation of the image into the MySql database.
                        InsertAnnotations(item.ID);//Adds annotation of the image into the MySql database.
                        newObs.Dispose();
                        count++;
                        if (Observation_pgbar.Value >= 100)//Checks weather the Observation_pgbar.Value is equal to 100 for images of next visit.
                            Observation_pgbar.Value = 0;
                        Observation_pgbar.Value += StepCount;//Calculates the Observation_pgbar value by adding the stepcount.
                        NoOfObs_lbl.Text = Observation_pgbar.Value.ToString() + "%";//Assigns the Observation_pgbar value to the NoOfObs_lbl.Text
                        NoOfObs_lbl.Refresh();
                    }
                    Observation_pgbar.Value = 100;
                    NoOfObs_lbl.Text = Observation_pgbar.Value.ToString() + "%";
                    NoOfObs_lbl.Refresh();
                }
            }
            catch (Exception ex) 
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }

            
        }

        /// <summary>
        /// Inserts the image details about the mask witdh,mask height,ImageCenterX and ImageCenterY into the database.
        /// </summary>
        /// <param name="newObs">Image</param>
        public void InsertObservationAttributes(obs newObs)
        {
            try
            {
                ObservationAttribute obsAttribute = ObservationAttribute.CreateNewObservationAttribute();
                ObservationAttributeType obsAttributetype = ObservationAttributeType.CreateNewObservationAttributeType();
                obsAttributetype.obsAttributeTypeId = (int)ObservationAttributeTypeEnum.MaskHeight;
                obsAttribute.attributeType = obsAttributetype;
                obsAttribute.observation = newObs;
                obsAttribute.value = IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._MaskHeight.val;
                NewDataVariables._Repo.Add<ObservationAttribute>(obsAttribute);//Inserts the mask height attribute into the mysql database.

                obsAttribute = ObservationAttribute.CreateNewObservationAttribute();
                obsAttributetype.obsAttributeTypeId = (int)ObservationAttributeTypeEnum.MaskWidth;
                obsAttribute.attributeType = obsAttributetype;
                obsAttribute.observation = newObs;
                obsAttribute.value = IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._MaskWidth.val;
                NewDataVariables._Repo.Add<ObservationAttribute>(obsAttribute);//Inserts the mask width attribute into the mysql database.

                obsAttribute = ObservationAttribute.CreateNewObservationAttribute();
                obsAttributetype.obsAttributeTypeId = (int)ObservationAttributeTypeEnum.ImageCenterX;
                obsAttribute.attributeType = obsAttributetype;
                obsAttribute.observation = newObs;
                obsAttribute.value = IVLVariables._ivlConfig.CameraSettings._ImageOpticalCentreX.val;
                NewDataVariables._Repo.Add<ObservationAttribute>(obsAttribute);//Inserts the Image CenterX attribute into the mysql database.

                obsAttribute = ObservationAttribute.CreateNewObservationAttribute();
                obsAttributetype.obsAttributeTypeId = (int)ObservationAttributeTypeEnum.ImageCenterY;
                obsAttribute.attributeType = obsAttributetype;
                obsAttribute.observation = newObs;
                obsAttribute.value = IVLVariables._ivlConfig.CameraSettings._ImageOpticalCentreY.val;
                NewDataVariables._Repo.Add<ObservationAttribute>(obsAttribute);//Inserts the Image CenterY attribute into the mysql database.
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Will Insert the report data from Sqlite to mysql.
        /// </summary>
        /// <param name="newVisit">visit data of type INTUSOFT.Data.NewDbModel.visit</param>
        /// <param name="oldVisit">visit data of type INTUSOFT.Data.Model.VisitModel</param>
        public void InsertReport(int oldVisitId)
        {
            try
            {
                {
                    int count = 1;
                    List<Report> oldReport = reports.Where(x => x.VisitID == oldVisitId).ToList<Report>();
                    int StepCount = 0;
                    if (oldReport.Count > 0)
                        StepCount = (int)100 / oldReport.Count;//Calculates the StepCount value of no of reports in the oldReport.
                    foreach (Report item in oldReport)//Adds the reports in list oldReport into the Mysql database.
                    {
                        InsertReports_lbl.Text = "Inserting report " + count;
                        InsertReports_lbl.Refresh();
                        newreport = INTUSOFT.Data.NewDbModel.report.CreateNewReport();
                        newreport.Patient = newVisit.patient;
                        newreport.visit = newVisit;
                        newreport.createdBy = user;
                        newreport.dataJson = item.ReportXML;
                        newreport.report_type = reportType;
                        if (item.ReportDateTime.Year != 1)
                            newreport.createdDate = item.ReportDateTime;
                        if (item.ReportModifyDateTime.Year != 1)
                            newreport.lastModifiedDate = item.ReportModifyDateTime;
                        if (item.HideShowRow == 1)
                            newreport.voided = true;
                        else if (item.HideShowRow == 0)
                            newreport.voided = false;

                        NewDataVariables._Repo.Add<report>(newreport);//Inserts the report into the Mysql database.
                        newreport.Dispose();
                        count++;

                        if (Report_pgbar.Value >= 100)//Checks weather the value is greater than or equal to 100 for the reports of the new visit.
                            Report_pgbar.Value = 0;
                        Report_pgbar.Value += StepCount;//Calculates the Report_pgbar value by adding the stepcount.
                        NoOfReports_lbl.Text = Report_pgbar.Value.ToString() + "%";//Assigns the Report_pgbar value to the NoOfReports_lbl.Text
                        NoOfReports_lbl.Refresh();
                    }
                    Report_pgbar.Value = 100;
                    NoOfReports_lbl.Text = Report_pgbar.Value.ToString() + "%";
                    NoOfReports_lbl.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Add the image data from old annotation to new annotation.
        /// </summary>
        /// <param name="newObs">Image data type INTUSOFT.Data.NewDbModel.obs </param>
        /// <param name="oldImage">Image data type INTUSOFT.Data.Model.Image</param>
        public void InsertAnnotations(int oldImageId)
        {
            try
            {
                int count = 1;
                newImage.eye_fundus_image_id = newObs.observationId;
                int StepCount = 0;
                List<AnnotationModel> oldAnnotation = annotation.Where(x => x.ImageID == oldImageId).ToList<AnnotationModel>();
                {
                    if (oldAnnotation.Count > 0)
                        StepCount = (int)100 / oldAnnotation.Count;//Calculates the StepCount value of no of annotation in the oldAnnotation.

                    foreach (AnnotationModel item in oldAnnotation)//Adds the annotation in list oldAnnotation into the Mysql database.
                    {
                        InsertAnnotation_lbl.Text = "Inserting annotation " + count;
                        InsertAnnotation_lbl.Refresh();
                        newAnnotation = INTUSOFT.Data.NewDbModel.eye_fundus_image_annotation.CreateNewEyeFundusImageAnnotation();
                        newAnnotation.comments = item.Comments;
                        newAnnotation.createdBy = user;
                        if (item.Date_Time.Year != 1)
                            newAnnotation.createdDate = item.Date_Time;
                        newAnnotation.voided = item.HideShowRow;
                        newAnnotation.dataXml = item.AnnotationXml;
                        if (isCdrPresent(item.AnnotationXml))//Checks weather the annotation id CDR or normal annotation.
                        {
                            newAnnotation.cdrPresent = true;
                        }
                        else
                        {
                            newAnnotation.cdrPresent = false;
                        }
                        newAnnotation.eyeFundusImage = newImage;
                        NewDataVariables._Repo.Add<eye_fundus_image_annotation>(newAnnotation);//Inserts the annotation into the Mysql Database
                        count++;
                        if (Annotation_pgbar.Value >= 100)//Checks weather the value is greater than or equal to 100 for the annotations of the next image.
                            Annotation_pgbar.Value = 0;
                        Annotation_pgbar.Value += StepCount;//Calculates the Annotation_pgbar value based on the step count.
                        NoOfAnnotation_lbl.Text = Annotation_pgbar.Value.ToString() + "%";//Assigns the  Annotation_pgbar.value to the .NoOfAnnotation_lbl.Text.
                        NoOfAnnotation_lbl.Refresh();
                    }
                    Annotation_pgbar.Value = 100;
                    NoOfAnnotation_lbl.Text = Annotation_pgbar.Value.ToString() + "%";//Assigns the  Annotation_pgbar.value to the .NoOfAnnotation_lbl.Text.
                    NoOfAnnotation_lbl.Refresh();
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        /// <summary>
        /// Will insert the data from sqlite database to mysql database.
        /// </summary>
        public void insertIntoNewDb()
        {
            InsertPatient();
        }

        /// <summary>
        /// Event which will read the data from the SQLite Database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openExisitingDb_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo finf = new FileInfo(ofd.FileName);
                    if (finf.Name.Equals("patient.db3"))//Checks weather proper file is selected or not.
                    {
                        NHibernateHelper.dbName = finf.Name.Split('.')[0];// "patient";
                        NHibernateHelper.dbfilePath = finf.DirectoryName;
                        pats = new List<INTUSOFT.Data.Model.Patient>();
                        visits = new List<VisitModel>();
                        image = new List<ImageModel>();
                        reports = new List<Report>();
                        annotation = new List<AnnotationModel>();
                        INTUSOFT.Data.Repository.OldRepositories.DataVariables._visitViewRepo = INTUSOFT.Data.Repository.OldRepositories.VisitRepository.GetInstance();
                        INTUSOFT.Data.Repository.OldRepositories.DataVariables._patientRepo = INTUSOFT.Data.Repository.OldRepositories.PatientRepository.GetInstance();
                        INTUSOFT.Data.Repository.OldRepositories.DataVariables._imageRepo = INTUSOFT.Data.Repository.OldRepositories.ImageRepository.GetInstance();
                        INTUSOFT.Data.Repository.OldRepositories.DataVariables._reportRepo = INTUSOFT.Data.Repository.OldRepositories.ReporttRepository.GetInstance();
                        INTUSOFT.Data.Repository.OldRepositories.DataVariables._annotationRepo = INTUSOFT.Data.Repository.OldRepositories.AnnotationRepository.GetInstance();
                        pats = INTUSOFT.Data.Repository.OldRepositories.DataVariables._patientRepo.GetAll().ToList<INTUSOFT.Data.Model.Patient>();
                        visits = INTUSOFT.Data.Repository.OldRepositories.DataVariables._visitViewRepo.GetAll().ToList<VisitModel>();
                        image = INTUSOFT.Data.Repository.OldRepositories.DataVariables._imageRepo.GetAll().ToList<ImageModel>();
                        reports = INTUSOFT.Data.Repository.OldRepositories.DataVariables._reportRepo.GetAll().ToList<Report>();
                        annotation = INTUSOFT.Data.Repository.OldRepositories.DataVariables._annotationRepo.GetAll().ToList<AnnotationModel>();
                        NHibernateHelper.CloseSession();
                        MessageBox.Show("Old DB data reading over");
                    }
                    else
                    {
                        MessageBox.Show("Please select a SQLite database patient.db3");
                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        public void OdDbReading()
        {
            try
            {
                FileInfo finf = new FileInfo("C:\\patient.db3");
                if (!File.Exists(finf.FullName))
                {
                    OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        finf = new FileInfo(ofd.FileName);
                    }
                }
                if (finf.Name.Equals("patient.db3"))
                {
                    NHibernateHelper.dbName = finf.Name.Split('.')[0];// "patient";
                    NHibernateHelper.dbfilePath = finf.DirectoryName;
                    pats = new List<INTUSOFT.Data.Model.Patient>();
                    visits = new List<VisitModel>();
                    image = new List<ImageModel>();
                    reports = new List<Report>();
                    annotation = new List<AnnotationModel>();
                    INTUSOFT.Data.Repository.OldRepositories.DataVariables._visitViewRepo = INTUSOFT.Data.Repository.OldRepositories.VisitRepository.GetInstance();
                    INTUSOFT.Data.Repository.OldRepositories.DataVariables._patientRepo = INTUSOFT.Data.Repository.OldRepositories.PatientRepository.GetInstance();
                    INTUSOFT.Data.Repository.OldRepositories.DataVariables._imageRepo = INTUSOFT.Data.Repository.OldRepositories.ImageRepository.GetInstance();
                    INTUSOFT.Data.Repository.OldRepositories.DataVariables._reportRepo = INTUSOFT.Data.Repository.OldRepositories.ReporttRepository.GetInstance();
                    INTUSOFT.Data.Repository.OldRepositories.DataVariables._annotationRepo = INTUSOFT.Data.Repository.OldRepositories.AnnotationRepository.GetInstance();
                    pats = INTUSOFT.Data.Repository.OldRepositories.DataVariables._patientRepo.GetAll().ToList<INTUSOFT.Data.Model.Patient>();
                    visits = INTUSOFT.Data.Repository.OldRepositories.DataVariables._visitViewRepo.GetAll().ToList<VisitModel>();
                    image = INTUSOFT.Data.Repository.OldRepositories.DataVariables._imageRepo.GetAll().ToList<ImageModel>();
                    reports = INTUSOFT.Data.Repository.OldRepositories.DataVariables._reportRepo.GetAll().ToList<Report>();
                    annotation = INTUSOFT.Data.Repository.OldRepositories.DataVariables._annotationRepo.GetAll().ToList<AnnotationModel>();
                    NHibernateHelper.CloseSession();
                    MessageBox.Show("Old DB data reading over");
                }
                else
                {
                    MessageBox.Show("Please select Patient.db3 in C drive");
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        private void insertIntoNewDb_btn_Click(object sender, EventArgs e)
        {
            insertIntoNewDb();
        }

        /// <summary>
        /// Checks wheather the string is annotation or CDR.
        /// </summary>
        /// <param name="value">AnnotationXml value</param>
        /// <returns>true if its CDR else false</returns>
        public bool isCdrPresent(string value)
        {
            bool isCDR = false;
            try
            {
                
                byte[] bytes = null;
                Annotation.AnnotationXMLProperties anno = null;
                //Below code in the try block has been retained to handle the situation when the annotation xml data is getting readed from old annotationxml format(binary format).
                try
                {
                    bytes = Convert.FromBase64String(value);
                    using (MemoryStream stream = new MemoryStream(bytes))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        anno = (Annotation.AnnotationXMLProperties)formatter.Deserialize(stream);
                    }
                    if (anno.isCupProperties != null)
                    {
                        if (anno.isCupProperties.Contains(true))
                        {
                            isCDR = true;
                        }
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        using (StringReader sr = new StringReader(value))
                        {
                            XmlReaderSettings settings = new XmlReaderSettings();
                            settings.ConformanceLevel = ConformanceLevel.Auto;
                            using (XmlReader xmlReader = XmlReader.Create(sr, settings))
                            {
                                {
                                    XmlSerializer xmlSer = new XmlSerializer(typeof(Annotation.AnnotationXMLProperties));
                                    anno = xmlSer.Deserialize(xmlReader) as Annotation.AnnotationXMLProperties;
                                }
                            }
                        }
                        if (anno.isCupProperties.Contains(true))
                        {
                            isCDR = true;
                        }
                    }
                    catch (Exception)
                    {
                        svgDoc = SvgDocument.FromSvg<SvgDocument>(value);
                        SvgElementCollection svg = svgDoc.Children;
                        anno = new Annotation.AnnotationXMLProperties();
                        //SvgElementCollection sg = new SvgElementCollection();
                        for (int i = 0; i < svg.Count; i++)
                        {
                            if (svg[i] is Svg.SvgPolygon)
                            {
                                SvgPolygon svgPolygon = svg[i] as SvgPolygon;
                                if (svgPolygon.ID == "cup")
                                {
                                    isCDR = true;
                                }
                            }
                        }
                        anno.isCupProperties.Reverse();
                    }
                }
                return isCDR;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
                return isCDR;

            }
            
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void SelectConfig_btn_Click(object sender, EventArgs e)
        {
            try
            {
                //timeProfiling.ShowDialog();
                OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FileInfo finf = new FileInfo(ofd.FileName);
                    if (finf.Name.Equals(ConfigFileName))
                    {
                        IVLConfig.fileName = ofd.FileName;
                        IVLVariables._ivlConfig = IVLConfig.getInstance();
                        ////List<INTUSOFT.Data.NewDbModel.Patient> samplePatients = NewDataVariables._Repo.GetPageData<INTUSOFT.Data.NewDbModel.Patient>(Convert.ToInt32(IVLVariables._ivlConfig.UserSettings._noOfPatientsPerPage.val), 1).ToList<INTUSOFT.Data.NewDbModel.Patient>();
                        ////MessageBox.Show(samplePatients.Count.ToString());
                        insertIntoNewDb_btn.Enabled = true;
                        //portingForm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Please select a Config file");
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
            //else
            //    return;
        }

        void SaveCaptureLog()
        {
            try
            {
                using (var sw = new StringWriter())
                {
                    XmlWriterSettings Settings = new XmlWriterSettings();
                    Settings.Encoding = Encoding.UTF8;
                    using (var xmlWriter = XmlWriter.Create(sw, Settings))
                    {
                        CaptureLog captureSettings = new CaptureLog();
                        captureSettings.currentLiveGain = Convert.ToUInt16(IVLVariables._ivlConfig.CameraSettings._LiveDigitalGain.val);
                        captureSettings.currentLiveExposure = Convert.ToUInt32(IVLVariables._ivlConfig.CameraSettings._Exposure.val);
                        captureSettings.currentFlashGain = Convert.ToUInt16(IVLVariables._ivlConfig.CameraSettings._DigitalGain.val);
                        captureSettings.currentFlashExposure = Convert.ToUInt32(IVLVariables._ivlConfig.CameraSettings._FlashExposure.val);
                        captureSettings.currentCameraType = ImagingMode.Posterior_45;
                        captureSettingsSerializer.Serialize(xmlWriter, captureSettings);
                        xmlWriter.Close();
                    }
                    captureSettingsValue = sw.ToString();
                }
                using (var sw = new StringWriter())
                {
                    XmlWriterSettings Settings = new XmlWriterSettings();
                    Settings.Encoding = Encoding.UTF8;
                    using (var xmlWriter = XmlWriter.Create(sw, Settings))
                    {
                        MaskSettings maskSettings = new MaskSettings();
                        maskSettings.maskHeight = Convert.ToInt32(IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._MaskHeight.val);
                        maskSettings.maskWidth = Convert.ToInt32(IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._MaskWidth.val);
                        maskSettings.maskCentreX = Convert.ToInt32(IVLVariables._ivlConfig.CameraSettings._ImageOpticalCentreX.val);
                        maskSettings.maskCentreY = Convert.ToInt32(IVLVariables._ivlConfig.CameraSettings._ImageOpticalCentreY.val);
                        maskSettings.isApplyLiveMask = Convert.ToBoolean(IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._ApplyLiveMask.val);
                        maskSettings.isApplyLogo = Convert.ToBoolean(IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._IsApplyMask.val);
                        maskSettings.isApplyMask = Convert.ToBoolean(IVLVariables._ivlConfig.PostProcessingSettings.MaskSettings._IsApplyMask.val);
                        maskSettingsSerializer.Serialize(xmlWriter, maskSettings);
                        xmlWriter.Close();
                    }
                    maskSettingsValue = sw.ToString(); ;
                }
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
            }
            
        }

        public int noOfPagesForAllPatients()
        {
            int noOfPages = 0;
            try
            {
                
                noOfPages = NewDataVariables.Patients.Count / Convert.ToInt32(IVLVariables._ivlConfig.UserSettings._noOfPatientsPerPage.val);
                return noOfPages;
            }
            catch (Exception ex)
            {
                Common.ExceptionLogWriter.WriteLog(ex, Exception_Log);
                return noOfPages;

            }
            
        }

        private void DbTransferForm_Load(object sender, EventArgs e)
        {
            OdDbReading();
        }
    }

    public class PersonAttributes
    {
        public string Income;
        public string Occupation;
        public string Landline;
        public string Weight;
        public string Height;
        public string Comments;

        public PersonAttributes()
        {

        }
    }


    public class MaskSettings
    {
        public bool isApplyMask = false;
        public bool isApplyLiveMask = false;
        public bool isApplyLogo = false;
        public int maskCentreX = 900;
        public int maskCentreY = 700;
        public int maskWidth = 1700;
        public int maskHeight = 1700;
        public MaskSettings()
        {
        }
    }

    public class CaptureLog
    {
        public ushort currentLiveGain;
        public uint currentLiveExposure;
        public ushort currentFlashGain;
        public uint currentFlashExposure;
        public ImagingMode currentCameraType;
        public CaptureLog()
        {

        }
    }
    public enum ImagingMode { Posterior_45, FFA, Anterior_Prime, Posterior_Prime, FFAColor, FFA_Plus, Anterior_FFA, Anterior_45 };
}
