-- MySQL Workbench Forward Engineering


SET FOREIGN_KEY_CHECKS=0;

-- -----------------------------------------------------
-- Schema dbName
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema dbName
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `dbName` DEFAULT CHARACTER SET utf8 ;
USE `dbName` ;

-- -----------------------------------------------------
-- Table `concept_class`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `concept_class` (
  `concept_class_id` SMALLINT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(300) NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`concept_class_id`),
  INDEX `concept_class_name_index` (`name` ASC),
  UNIQUE INDEX `concept_class_uuid_unique_index` (`uuid` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `concept_datatype`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `concept_datatype` (
  `concept_datatype_id` SMALLINT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `hl7_abbreviation` CHAR(3) NULL,
  `description` VARCHAR(300) NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`concept_datatype_id`),
  INDEX `concept_datatype_name_index` (`name` ASC),
  UNIQUE INDEX `concept_datatype_uuid_unique_index` (`uuid` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `concept`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `concept` (
  `concept_id` BIGINT NOT NULL AUTO_INCREMENT,
  `short_name` VARCHAR(200) NULL,
  `description` VARCHAR(500) NULL,
  `fully_specified_name` VARCHAR(300) NOT NULL,
  `is_set` TINYINT(1) NOT NULL DEFAULT 0,
  `datatype_id` SMALLINT NOT NULL,
  `class_id` SMALLINT NOT NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`concept_id`),
  INDEX `concept_belongs_to_class_index` (`class_id` ASC),
  INDEX `concept_belongs_to_datatype_index` (`datatype_id` ASC),
  UNIQUE INDEX `concept_uuid_unique_index` (`uuid` ASC),
  CONSTRAINT `concept_belongs_to_class_fk`
    FOREIGN KEY (`class_id`)
    REFERENCES `concept_class` (`concept_class_id`),
  CONSTRAINT `concept_belongs_to_datatype_fk`
    FOREIGN KEY (`datatype_id`)
    REFERENCES `concept_datatype` (`concept_datatype_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `concept_complex`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `concept_complex` (
  `concept_id` BIGINT NOT NULL,
  `handler_name` VARCHAR(300) NOT NULL,
  PRIMARY KEY (`concept_id`),
  CONSTRAINT `concept_complex_belongs_to_concept_fk`
    FOREIGN KEY (`concept_id`)
    REFERENCES `concept` (`concept_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `global_property`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `global_property` (
  `global_property_id` VARCHAR(100) NOT NULL,
  `value` VARCHAR(300) NOT NULL,
  `description` VARCHAR(300) NULL,
  PRIMARY KEY (`global_property_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `person`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person` (
  `person_id` BIGINT NOT NULL AUTO_INCREMENT,
  `first_name` VARCHAR(50) NULL,
  `middle_name` VARCHAR(50) NULL,
  `last_name` VARCHAR(50) NULL,
  `gender` CHAR(1) NULL,
  `birthdate` DATE NULL,
  `birthdate_estimated` TINYINT(1) NULL DEFAULT 0,
  `profile_image` BLOB NULL,
  `primary_phone_number` VARCHAR(17) NULL,
  `primary_email_id` VARCHAR(150) NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `last_accessed_by` BIGINT NULL,
  `last_accessed_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT 0,
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`person_id`),
  INDEX `person_created_by_index` (`created_by` ASC),
  INDEX `person_voided_by_user_index` (`voided_by` ASC),
  INDEX `person_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `person_birthdate_index` (`birthdate` ASC),
  UNIQUE INDEX `person_uuid_unique_index` (`uuid` ASC),
  INDEX `person_first_name_index` (`first_name` ASC),
  INDEX `person_middle_name_index` (`middle_name` ASC),
  INDEX `person_last_name_index` (`last_name` ASC),
  INDEX `person_gender_index` (`gender` ASC),
  INDEX `person_accessed_by_user_index` (`last_accessed_by` ASC),
  CONSTRAINT `person_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `person_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `person_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `person_accessed_by_user_fk`
    FOREIGN KEY (`last_accessed_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `users`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `users` (
  `user_id` BIGINT NOT NULL AUTO_INCREMENT,
  `system_id` VARCHAR(50) NOT NULL,
  `person_id` BIGINT NOT NULL,
  `username` VARCHAR(50) NOT NULL,
  `password` CHAR(60) NULL,
  `secret_question` VARCHAR(300) NULL,
  `secret_answer` VARCHAR(60) NULL,
  `created_by` BIGINT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `retired` TINYINT(1) NOT NULL DEFAULT '0',
  `retired_by` BIGINT NULL,
  `retired_date` DATETIME NULL,
  `retired_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`user_id`),
  INDEX `user_created_by_user_index` (`created_by` ASC),
  INDEX `user_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `user_is_person_index` (`person_id` ASC),
  INDEX `user_retired_by_user_index` (`retired_by` ASC),
  UNIQUE INDEX `user_uuid_unique_index` (`uuid` ASC),
  UNIQUE INDEX `user_username_unique_index` (`username` ASC),
  UNIQUE INDEX `user_system_id_unique_index` (`system_id` ASC),
  CONSTRAINT `user_is_person_fk`
    FOREIGN KEY (`person_id`)
    REFERENCES `person` (`person_id`),
  CONSTRAINT `user_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `user_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `user_retired_by_user_fk`
    FOREIGN KEY (`retired_by`)
    REFERENCES `users` (`user_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `patient`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `patient` (
  `patient_id` BIGINT NOT NULL,
  `history_ailments` VARCHAR(1000) NULL,
  `referred_by` BIGINT NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT '0',
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `last_sent_by` BIGINT NULL,
  `last_sent_date` DATETIME NULL,
  PRIMARY KEY (`patient_id`),
  INDEX `patient_created_by_user_index` (`created_by` ASC),
  INDEX `patient_voided_by_user_index` (`voided_by` ASC),
  INDEX `patient_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `patient_referred_by_person_index` (`referred_by` ASC),
  INDEX `patient_sent_by_user_index` (`last_sent_by` ASC),
  CONSTRAINT `patient_is_person_fk`
    FOREIGN KEY (`patient_id`)
    REFERENCES `person` (`person_id`)
    ON UPDATE CASCADE,
  CONSTRAINT `patient_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_referred_by_person_fk`
    FOREIGN KEY (`referred_by`)
    REFERENCES `person` (`person_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `patient_sent_by_user_fk`
    FOREIGN KEY (`last_sent_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `visit`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `visit` (
  `visit_id` BIGINT NOT NULL AUTO_INCREMENT,
  `patient_id` BIGINT NOT NULL,
  `start_datetime` DATETIME NOT NULL,
  `end_datetime` DATETIME NULL,
  `created_by` BIGINT NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT '0',
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `last_accessed_by` BIGINT NULL,
  `last_accessed_date` DATETIME NULL,
  `last_sent_by` BIGINT NULL,
  `last_sent_date` DATETIME NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`visit_id`),
  INDEX `visit_created_by_user_index` (`created_by` ASC),
  INDEX `visit_voided_by_user_index` (`voided_by` ASC),
  INDEX `visit_belongs_to_patient_index` (`patient_id` ASC),
  INDEX `visit_accessed_by_user_index` (`last_accessed_by` ASC),
  INDEX `visit_sent_by_user_index` (`last_sent_by` ASC),
  UNIQUE INDEX `visit_uuid_unique_index` (`uuid` ASC),
  INDEX `visit_modified_by_user_index` (`last_modified_by` ASC),
  CONSTRAINT `visit_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `visit_belongs_to_patient_fk`
    FOREIGN KEY (`patient_id`)
    REFERENCES `patient` (`patient_id`),
  CONSTRAINT `visit_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `visit_accessed_by_user_fk`
    FOREIGN KEY (`last_accessed_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `visit_sent_by_user_fk`
    FOREIGN KEY (`last_sent_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `visit_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `observation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `observation`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `observation` (
  `observation_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `concept_id` bigint(20) NOT NULL,
  `patient_id` bigint(20) NOT NULL,
  `visit_id` bigint(20) DEFAULT NULL,
  `taken_datetime` datetime NOT NULL,
  `group_id` bigint(20) DEFAULT NULL,
  `value` varchar(500) DEFAULT NULL,
  `comment` varchar(300) DEFAULT NULL,
  `created_by` bigint(20) NOT NULL,
  `created_date` datetime NOT NULL,
  `last_modified_by` bigint(20) DEFAULT NULL,
  `last_modified_date` datetime DEFAULT NULL,
  `voided` tinyint(1) NOT NULL DEFAULT '0',
  `voided_by` bigint(20) DEFAULT NULL,
  `voided_date` datetime DEFAULT NULL,
  `voided_reason` varchar(300) DEFAULT NULL,
  `uuid` char(38) DEFAULT NULL,
  `last_sent_by` bigint(20) DEFAULT NULL,
  `last_sent_date` datetime DEFAULT NULL,
  PRIMARY KEY (`observation_id`),
  UNIQUE KEY `observation_uuid_unique_index` (`uuid`),
  KEY `observation_belongs_to_concept_index` (`concept_id`),
  KEY `observation_created_by_user_index` (`created_by`),
  KEY `observation_voided_by_user_index` (`voided_by`),
  KEY `observation_belongs_to_observation_group_index` (`group_id`),
  KEY `observation_belongs_to_visit_index` (`visit_id`),
  KEY `observation_modified_by_user_index` (`last_modified_by`),
  KEY `observation_sent_by_user_index` (`last_sent_by`),
  KEY `observation_belongs_to_person_fk_idx` (`patient_id`),
  CONSTRAINT `observation_belongs_to_concept_fk` FOREIGN KEY (`concept_id`) REFERENCES `concept` (`concept_id`),
  CONSTRAINT `observation_belongs_to_observation_group_fk` FOREIGN KEY (`group_id`) REFERENCES `observation` (`observation_id`),
  CONSTRAINT `observation_belongs_to_patient_fk` FOREIGN KEY (`patient_id`) REFERENCES `patient` (`patient_id`),
  CONSTRAINT `observation_belongs_to_visit_fk` FOREIGN KEY (`visit_id`) REFERENCES `visit` (`visit_id`),
  CONSTRAINT `observation_created_by_user_fk` FOREIGN KEY (`created_by`) REFERENCES `users` (`user_id`),
  CONSTRAINT `observation_modified_by_user_fk` FOREIGN KEY (`last_modified_by`) REFERENCES `users` (`user_id`),
  CONSTRAINT `observation_sent_by_user_fk` FOREIGN KEY (`last_sent_by`) REFERENCES `users` (`user_id`),
  CONSTRAINT `observation_voided_by_user_fk` FOREIGN KEY (`voided_by`) REFERENCES `users` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


-- -----------------------------------------------------
-- Table `note`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `note` (
  `note_id` BIGINT NOT NULL AUTO_INCREMENT,
  `observation_id` BIGINT NULL,
  `visit_id` BIGINT NULL,
  `patient_id` BIGINT NULL,
  `text` TEXT NOT NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT 0,
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`note_id`),
  INDEX `note_belongs_to_obs_index` (`observation_id` ASC),
  INDEX `note_created_by_user_index` (`created_by` ASC),
  INDEX `note_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `note_which_belongs_to_visit_index` (`visit_id` ASC),
  INDEX `note_belongs_to_patient_index` (`patient_id` ASC),
  UNIQUE INDEX `note_uuid_unique_index` (`uuid` ASC),
  INDEX `note_voided_by_user_idx` (`voided_by` ASC),
  CONSTRAINT `note_belongs_to_obs_fk`
    FOREIGN KEY (`observation_id`)
    REFERENCES `observation` (`observation_id`),
  CONSTRAINT `note_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `note_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `note_which_belongs_to_visit_fk`
    FOREIGN KEY (`visit_id`)
    REFERENCES `visit` (`visit_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `note_belongs_to_patient_fk`
    FOREIGN KEY (`patient_id`)
    REFERENCES `patient` (`patient_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `note_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `patient_identifier_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `patient_identifier_type` (
  `patient_identifier_type_id` SMALLINT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `required` TINYINT(1) NOT NULL DEFAULT '0',
  `format` VARCHAR(300) NULL,
  `format_description` VARCHAR(300) NULL,
  `validator` VARCHAR(300) NULL,
  `description` VARCHAR(300) NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `retired` TINYINT(1) NOT NULL DEFAULT '0',
  `retired_by` BIGINT NULL,
  `retired_date` DATETIME NULL,
  `retired_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`patient_identifier_type_id`),
  INDEX `patient_identifier_type_created_by_user_index` (`created_by` ASC),
  INDEX `patient_identifier_type_retired_by_user_index` (`retired_by` ASC),
  INDEX `patient_identifier_type_modified_by_user_index` (`last_modified_by` ASC),
  UNIQUE INDEX `patient_identifier_type_uuid_unique_index` (`uuid` ASC),
  CONSTRAINT `patient_identifier_type_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_identifier_type_retired_by_user_fk`
    FOREIGN KEY (`retired_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_identifier_type_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `patient_identifier`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `patient_identifier` (
  `patient_identifier_id` BIGINT NOT NULL AUTO_INCREMENT,
  `patient_id` BIGINT NOT NULL,
  `value` VARCHAR(30) NOT NULL,
  `identifier_type_id` SMALLINT NOT NULL,
  `preferred` TINYINT(1) NOT NULL DEFAULT '0',
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT '0',
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`patient_identifier_id`),
  INDEX `patient_identifier_belongs_to_type_index` (`identifier_type_id` ASC),
  INDEX `patient_identifier_created_by_user_index` (`created_by` ASC),
  INDEX `patient_identifier_voided_by_user_index` (`voided_by` ASC),
  INDEX `patient_identifier_value_index` (`value` ASC),
  INDEX `patient_identifier_belongs_to_patient_index` (`patient_id` ASC),
  INDEX `patient_identifier_modified_by_user_index` (`last_modified_by` ASC),
  UNIQUE INDEX `patient_identifier_uuid_unique_index` (`uuid` ASC),
  CONSTRAINT `patient_identifier_belongs_to_type_fk`
    FOREIGN KEY (`identifier_type_id`)
    REFERENCES `patient_identifier_type` (`patient_identifier_type_id`),
  CONSTRAINT `patient_identifier_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_identifier_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `patient_identifier_belongs_to_patient_fk`
    FOREIGN KEY (`patient_id`)
    REFERENCES `patient` (`patient_id`),
  CONSTRAINT `patient_identifier_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `person_address`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_address` (
  `person_address_id` BIGINT NOT NULL AUTO_INCREMENT,
  `person_id` BIGINT NULL,
  `preferred` TINYINT(1) NOT NULL DEFAULT '0',
  `line1` VARCHAR(100) NULL,
  `line2` VARCHAR(100) NULL,
  `city_village` VARCHAR(50) NULL,
  `state_province` VARCHAR(50) NULL,
  `county_district` VARCHAR(50) NULL,
  `country` VARCHAR(50) NULL,
  `postal_code` VARCHAR(10) NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT '0',
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`person_address_id`),
  INDEX `person_address_created_by_user_index` (`created_by` ASC),
  INDEX `person_address_belongs_to_person_index` (`person_id` ASC),
  INDEX `person_address_voided_by_user_index` (`voided_by` ASC),
  UNIQUE INDEX `person_uuid_unique_index` (`uuid` ASC),
  INDEX `person_address_modified_by_user_index` (`last_modified_by` ASC),
  CONSTRAINT `person_address_belongs_to_person_fk`
    FOREIGN KEY (`person_id`)
    REFERENCES `person` (`person_id`)
    ON UPDATE CASCADE,
  CONSTRAINT `person_address_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `person_address_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`),
  CONSTRAINT `person_address_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `person_attribute_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_attribute_type` (
  `person_attribute_type_id` SMALLINT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(300) NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`person_attribute_type_id`),
  UNIQUE INDEX `person_attribute_type_uuid_unique_index` (`uuid` ASC),
  UNIQUE INDEX `person_attribute_type_name_unique_index` (`name` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `person_attribute`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `person_attribute` (
  `person_attribute_id` BIGINT NOT NULL AUTO_INCREMENT,
  `person_id` BIGINT NOT NULL,
  `value` VARCHAR(100) NULL,
  `person_attribute_type_id` SMALLINT NOT NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`person_attribute_id`),
  INDEX `person_attribute_belongs_to_person_index` (`person_id` ASC),
  INDEX `person_attribute_belongs_to_type_index` (`person_attribute_type_id` ASC),
  UNIQUE INDEX `person_attribute_uuid_unique_index` (`uuid` ASC),
  CONSTRAINT `person_attribute_belongs_to_type_fk`
    FOREIGN KEY (`person_attribute_type_id`)
    REFERENCES `person_attribute_type` (`person_attribute_type_id`),
  CONSTRAINT `person_attribute_belongs_to_person_fk`
    FOREIGN KEY (`person_id`)
    REFERENCES `person` (`person_id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `machine`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `machine` (
  `machine_id` SMALLINT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(300) NULL,
  `serial_number` VARCHAR(50) NOT NULL,
  `model_number` VARCHAR(50) NOT NULL,
  `created_by` BIGINT NULL,
  `created_date` DATETIME NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `retired` TINYINT(1) NOT NULL DEFAULT 0,
  `retired_by` BIGINT NULL,
  `retired_date` DATETIME NULL,
  `retired_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`machine_id`),
  UNIQUE INDEX `machine_uuid_unique_index` (`uuid` ASC),
  INDEX `machine_created_by_user_index` (`created_by` ASC),
  INDEX `machine_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `machine_retired_by_user_index` (`retired_by` ASC),
  CONSTRAINT `machine_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `machine_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `machine_retired_by_user_fk`
    FOREIGN KEY (`retired_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `organization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `organization` (
  `property_id` VARCHAR(100) NOT NULL,
  `value` VARCHAR(300) NOT NULL,
  PRIMARY KEY (`property_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `eye_fundus_image`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `eye_fundus_image` (
  `eye_fundus_image_id` BIGINT NOT NULL,
  `eye_side` CHAR(1) NOT NULL,
  `dilated_eye` TINYINT(1) NOT NULL,
  `last_accessed_by` BIGINT NULL,
  `last_accessed_date` DATETIME NULL,
  `camera_settings` TEXT NULL,
  `mask_settings` TEXT NULL,
  `machine_id` SMALLINT NOT NULL,
  PRIMARY KEY (`eye_fundus_image_id`),
  INDEX `eye_fundus_image_accessed_by_user_index` (`last_accessed_by` ASC),
  INDEX `eye_fundus_image_captured_by_machine_index` (`machine_id` ASC),
  CONSTRAINT `eye_fundus_image_accessed_by_user_fk`
    FOREIGN KEY (`last_accessed_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `eye_fundus_image_captured_by_machine_fk`
    FOREIGN KEY (`machine_id`)
    REFERENCES `machine` (`machine_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `eye_fundus_image_belongs_to_observation_fk`
    FOREIGN KEY (`eye_fundus_image_id`)
    REFERENCES `observation` (`observation_id`)
    ON DELETE RESTRICT
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `eye_fundus_image_annotation`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `eye_fundus_image_annotation` (
  `eye_fundus_image_annotation_id` BIGINT NOT NULL AUTO_INCREMENT,
  `eye_fundus_image_id` BIGINT NOT NULL,
  `comment` VARCHAR(85) NULL,
  `data_xml` TEXT NULL,
  `cdr_present` TINYINT(1) NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL DEFAULT 0,
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`eye_fundus_image_annotation_id`),
  INDEX `eye_fundus_image_annotation_belongs_to_image_index` (`eye_fundus_image_id` ASC),
  INDEX `eye_fundus_image_annotation_created_by_user_index` (`created_by` ASC),
  INDEX `eye_fundus_image_annotation_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `eye_fundus_image_annotation_voided_by_user_index` (`voided_by` ASC),
  UNIQUE INDEX `eye_fundus_image_annotation_uuid_unique_index` (`uuid` ASC),
  CONSTRAINT `eye_fundus_image_annotation_belongs_to_image_fk`
    FOREIGN KEY (`eye_fundus_image_id`)
    REFERENCES `eye_fundus_image` (`eye_fundus_image_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `eye_fundus_image_annotation_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `eye_fundus_image_annotation_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `eye_fundus_image_annotation_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `role` (
  `role_id` VARCHAR(50) NOT NULL,
  `description` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE INDEX `role_uuid_unique_index` (`uuid` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `privilege`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `privilege` (
  `privilege_id` VARCHAR(200) NOT NULL,
  `description` VARCHAR(300) NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`privilege_id`),
  UNIQUE INDEX `privilege_uuid_unique_index` (`uuid` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `role_role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `role_role` (
  `parent_role_id` VARCHAR(50) NOT NULL,
  `child_role_id` VARCHAR(50) NOT NULL,
  INDEX `role_parent_role_index` (`parent_role_id` ASC),
  INDEX `role_child_role_index` (`child_role_id` ASC),
  PRIMARY KEY (`parent_role_id`, `child_role_id`),
  CONSTRAINT `role_parent_role_fk`
    FOREIGN KEY (`parent_role_id`)
    REFERENCES `role` (`role_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `role_child_role_fk`
    FOREIGN KEY (`child_role_id`)
    REFERENCES `role` (`role_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `role_privilege`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `role_privilege` (
  `role_id` VARCHAR(50) NOT NULL,
  `privilege_id` VARCHAR(200) NOT NULL,
  INDEX `privilege_belongs_to_role_index` (`privilege_id` ASC),
  PRIMARY KEY (`role_id`, `privilege_id`),
  INDEX `role_has_privilege_index` (`role_id` ASC),
  CONSTRAINT `role_has_privilege_fk`
    FOREIGN KEY (`role_id`)
    REFERENCES `role` (`role_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `privilege_belongs_to_role_fk`
    FOREIGN KEY (`privilege_id`)
    REFERENCES `privilege` (`privilege_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `user_role`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `user_role` (
  `user_id` BIGINT NOT NULL,
  `role_id` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`user_id`, `role_id`),
  INDEX `role_belongs_to_user_index` (`role_id` ASC),
  INDEX `user_has_role_index` (`user_id` ASC),
  CONSTRAINT `user_has_role_fk`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `role_belongs_to_user_fk`
    FOREIGN KEY (`role_id`)
    REFERENCES `role` (`role_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `user_property`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `user_property` (
  `user_id` BIGINT NOT NULL,
  `property_id` VARCHAR(100) NOT NULL,
  `value` VARCHAR(300) NOT NULL,
  PRIMARY KEY (`user_id`, `property_id`),
  CONSTRAINT `user_property_belongs_to_user_fk`
    FOREIGN KEY (`user_id`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `concept_set`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `concept_set` (
  `concept_set_id` BIGINT NOT NULL AUTO_INCREMENT,
  `aggregator_concept_id` BIGINT NOT NULL,
  `member_concept_id` BIGINT NOT NULL,
  `sort_weight` DOUBLE NOT NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`concept_set_id`),
  INDEX `concept_set_has_a_concept_index` (`aggregator_concept_id` ASC),
  INDEX `concept_set_is_a_concept_index` (`member_concept_id` ASC),
  UNIQUE INDEX `concept_set_uuid_unique_index` (`uuid` ASC),
  CONSTRAINT `concept_set_has_a_concept_fk`
    FOREIGN KEY (`aggregator_concept_id`)
    REFERENCES `concept` (`concept_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `concept_set_is_a_concept_fk`
    FOREIGN KEY (`member_concept_id`)
    REFERENCES `concept` (`concept_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `report_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `report_type` (
  `report_type_id` SMALLINT NOT NULL,
  `name` VARCHAR(50) NOT NULL,
  `description` VARCHAR(300) NULL,
  `uuid` CHAR(38) NOT NULL,
  PRIMARY KEY (`report_type_id`),
  UNIQUE INDEX `report_uuid_unique_index` (`uuid` ASC))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `report`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `report` (
  `report_id` BIGINT NOT NULL AUTO_INCREMENT,
  `patient_id` BIGINT NOT NULL,
  `visit_id` BIGINT NOT NULL,
  `report_type_id` SMALLINT NOT NULL,
  `data_json` TEXT NULL,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NULL,
  `last_modified_by` BIGINT NULL,
  `last_modified_date` DATETIME NULL,
  `voided` TINYINT(1) NOT NULL,
  `voided_by` BIGINT NULL,
  `voided_date` DATETIME NULL,
  `voided_reason` VARCHAR(300) NULL,
  `uuid` CHAR(38) NULL,
  PRIMARY KEY (`report_id`),
  UNIQUE INDEX `report_uuid_unique_index` (`uuid` ASC),
  INDEX `report_belongs_to_visit_index` (`visit_id` ASC),
  INDEX `report_created_by_user_index` (`created_by` ASC),
  INDEX `report_modified_by_user_index` (`last_modified_by` ASC),
  INDEX `report_voided_by_user_index` (`voided_by` ASC),
  INDEX `report_belongs_to_type_index` (`report_type_id` ASC),
  INDEX `report_belongs_to_patient_index` (`patient_id` ASC),
  CONSTRAINT `report_belongs_to_visit_fk`
    FOREIGN KEY (`visit_id`)
    REFERENCES `visit` (`visit_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `report_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `report_modified_by_user_fk`
    FOREIGN KEY (`last_modified_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `report_voided_by_user_fk`
    FOREIGN KEY (`voided_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `report_belongs_to_type_fk`
    FOREIGN KEY (`report_type_id`)
    REFERENCES `report_type` (`report_type_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `report_belongs_to_patient_fk`
    FOREIGN KEY (`patient_id`)
    REFERENCES `patient` (`patient_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `sync_outbox`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `sync_outbox` (
  `record_id` BIGINT NOT NULL AUTO_INCREMENT,
  `type` VARCHAR(50) NOT NULL,
  `state` CHAR(38) NOT NULL DEFAULT 'In Queue',
  `status` TEXT NULL,
  `payload` LONGTEXT NULL,
  `object_id` BIGINT NOT NULL,
  `parent_id` BIGINT NULL,
  `retry_count` SMALLINT NOT NULL DEFAULT 0,
  `created_by` BIGINT NOT NULL,
  `created_date` DATETIME NOT NULL,
  `session_id` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`record_id`),
  INDEX `sync_outbox_record_created_by_user_index` (`created_by` ASC),
  INDEX `sync_outbox_parent_record_index` (`parent_id` ASC),
  CONSTRAINT `sync_outbox_record_created_by_user_fk`
    FOREIGN KEY (`created_by`)
    REFERENCES `users` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `sync_outbox_parent_record_fk`
    FOREIGN KEY (`parent_id`)
    REFERENCES `sync_outbox` (`record_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;
DROP TABLE IF EXISTS `patient_diagnosis`;
CREATE TABLE `patient_diagnosis` (
  `patient_diagnosis_id` int(11) NOT NULL AUTO_INCREMENT,
  `patient_id` bigint(20) NOT NULL,
  `visit_id` bigint(20) NOT NULL,
  `concept_id` bigint(20) NOT NULL,
  `diagnosis_value_left` varchar(2000) DEFAULT NULL,
  `diagnosis_value_right` varchar(2000) DEFAULT NULL,
  `created_date` datetime DEFAULT NULL,
  `voided_date` datetime DEFAULT NULL,
  `last_modified_date` datetime DEFAULT NULL,
  `voided` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`patient_diagnosis_id`),
  KEY `fk_idx` (`patient_id`),
  KEY `patient_diagnosis_concept_id_idx` (`concept_id`),
  KEY `voided_fk_idx` (`voided`),
  CONSTRAINT `concept_id_fk` FOREIGN KEY (`concept_id`) REFERENCES `concept` (`concept_id`),
  CONSTRAINT `patient_diagnosis_pat_id` FOREIGN KEY (`patient_id`) REFERENCES `patient` (`patient_id`),
    CONSTRAINT `visit_id_fk` FOREIGN KEY (`visit_id`) REFERENCES `visit` (`visit_id`) ON DELETE NO ACTION ON UPDATE NO ACTION

) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- -----------------------------------------------------
-- Data for table `concept_class`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `sp1`;
DELIMITER $$
CREATE PROCEDURE `sp1`()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `concept_class` into myId;
if(myId = 0)
then
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (1, 'Test', 'Acq. during patient encounter (vitals, labs, etc.)', '8d4907b2-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (2, 'Procedure', 'Describes a clinical procedure', '8d490bf4-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (3, 'Drug', 'Drug', '8d490dfc-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (4, 'Diagnosis', 'Conclusion drawn through findings', '8d4918b0-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (5, 'Finding', 'Practitioner observation/finding', '8d491a9a-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (6, 'Anatomy', 'Anatomic sites / descriptors', '8d491c7a-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (7, 'Question', 'Question (eg, patient history, SF36 items)', '8d491e50-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (8, 'LabSet', 'Term to describe laboratory sets', '8d492026-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (9, 'MedSet', 'Term to describe medication sets', '8d4923b4-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (10, 'ConvSet', 'Term to describe convenience sets', '8d492594-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (11, 'Misc', 'Terms which dont fit other categories', '8d492774-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (12, 'Symptom', 'Patient-reported observation', '8d492954-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (13, 'Symptom/Finding', 'Observation that can be reported from patient or found on exam', '8d492b2a-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (14, 'Specimen', 'Body or fluid specimen', '8d492d0a-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (15, 'Misc Order', 'Orderable items which arent tests or drugs', '8d492ee0-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_class` (`concept_class_id`, `name`, `description`, `uuid`) VALUES (16, 'Frequency', 'A class for order frequencies', '8e071bfe-520c-44c0-a89b-538e9129b42a');
END if;
END $$
DELIMITER ;
call sp1();
COMMIT;


-- -----------------------------------------------------
-- Data for table `concept_datatype`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `conceptDatatypeProc`;
DELIMITER $$
CREATE PROCEDURE `conceptDatatypeProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `concept_datatype` into myId;
if(myId = 0)
then
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (1, 'Numeric', 'NM', 'Numeric value, including integer or float (e.g., creatinine, weight)', '8d4a4488-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (2, 'Coded', 'CWE', 'Value determined by term dictionary lookup (i.e., term identifier)', '8d4a48b6-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (3, 'Text', 'ST', 'Free text', '8d4a4ab4-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (4, 'N/A', 'ZZ', 'Not associated with a datatype (e.g., term answers, sets)', '8d4a4c94-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (5, 'Document', 'RP', 'Pointer to a binary or text-based document (e.g., clinical document, RTF, XML, EKG, image, etc.) stored in complex_obs table', '8d4a4e74-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (6, 'Date', 'DT', 'Absolute date', '8d4a505e-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (7, 'Time', 'TM', 'Absolute time of day', '8d4a591e-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (8, 'Datetime', 'TS', 'Absolute date and time', '8d4a5af4-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (10, 'Boolean', 'BIT', 'Boolean value (yes/no, true/false)', '8d4a5cca-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (11, 'Rule', 'ZZ', 'Value derived from other data', '8d4a5e96-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (12, 'Structured Numeric', 'SN', 'Complex numeric values possible (ie, <5, 1-10, etc.)', '8d4a606c-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `concept_datatype` (`concept_datatype_id`, `name`, `hl7_abbreviation`, `description`, `uuid`) VALUES (13, 'Complex', 'ED', 'Complex value.  Analogous to HL7 Embedded Datatype', '8d4a6242-c2cc-11de-8d13-0010c6dffd0f');
END if;
END $$
DELIMITER ;
call conceptDatatypeProc();
COMMIT;

-- -----------------------------------------------------
-- Data for table `concept`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `conceptProc`;
DELIMITER $$
CREATE PROCEDURE `conceptProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `concept` into myId;
if(myId = 0)
then
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (1, 'Yes', '', 'TRUE', 0, 4, 11, '87e0d19a-e0f1-4a10-8c86-15679dc4ce01');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (2, 'No', '', 'FALSE', 0, 4, 11, '7fa51664-0061-4a0c-8d9c-e06d4e679a3f');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (3, 'Eye Fundus Image', '', 'EYE FUNDUS IMAGE', 0, 13, 1, '2d08c4dc-365d-4f03-94e9-fb218f185d11');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (4, 'Eye Fundus Images', '', 'EYE FUNDUS IMAGES', 1, 4, 8, '8797f3e0-668f-4e5d-9921-746a04e205ff');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (5, 'Lens Axis', '', 'LENS AXIS', 0, 1, 1, 'cf70537e-be98-42d1-a742-c1509ea8c58e');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (6, 'Lens Cylinder', '', 'LENS CYLINDER', 0, 1, 1, '5d578dfb-fd2f-45bc-80f9-21c8940df181');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (7, 'Lens Sphere', '', 'LENS SPHERE', 0, 1, 1, '7c7a7d42-3b1a-412e-b6d1-0fdbb2c4d533');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (8, 'Pupil Distance', '', 'PUPIL DISTANCE', 0, 1, 1, '8b86f9aa-fcf1-4619-8d03-5312797d9310');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (9, 'Left Eye Lens Prescription (os)', '', 'LEFT EYE LENS PRESCRIPTION (OS)', 1, 4, 8, 'f80d31b6-0dce-471b-95e9-63b232da4264');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (10, 'Right Eye Lens Prescription (od)', '', 'RIGHT EYE LENS PRESCRIPTION (OD)', 1, 4, 8, '993ce5a1-9f69-4d09-af42-0c1919ad010a');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (11, 'Eye Lens Prescription', '', 'EYE LENS PRESCRIPTION', 1, 4, 8, '8a29dd87-b2ab-4a1a-8462-4ce57a9a1c28');
INSERT INTO `concept` (`concept_id`, `short_name`, `description`, `fully_specified_name`, `is_set`, `datatype_id`, `class_id`, `uuid`) VALUES (12,'EyeFundusDiagnosis','','EYE FUNDUS DIAGNOSIS',0,3,4,'8a29dd87-b2ab-4a1a-8462-4ce57a9a1c26');
END if;
END $$
DELIMITER ;
call conceptProc();
COMMIT;


-- -----------------------------------------------------
-- Data for table `concept_complex`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `conceptComplexProc`;
DELIMITER $$
CREATE PROCEDURE `conceptComplexProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `concept_complex` into myId;
if(myId = 0)
then
INSERT INTO `concept_complex` (`concept_id`, `handler_name`) VALUES (3, 'EyeFundusImageHandler');
END if;
END $$
DELIMITER ;
call conceptComplexProc();
COMMIT;


-- -----------------------------------------------------
-- Data for table `person`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `personProc`;
DELIMITER $$
CREATE PROCEDURE `personProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `person` into myId;
if(myId = 0)
then
INSERT INTO `person` (`person_id`, `first_name`, `middle_name`, `last_name`, `gender`, `birthdate`, `birthdate_estimated`, `profile_image`, `primary_phone_number`, `primary_email_id`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `last_accessed_by`, `last_accessed_date`, `voided`, `voided_by`, `voided_date`, `voided_reason`, `uuid`) VALUES (1, 'system', NULL, 'system', NULL, NULL, 0, NULL, NULL, NULL, 1, '2016-01-01 00:00:00', NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL);
INSERT INTO `person` (`person_id`, `first_name`, `middle_name`, `last_name`, `gender`, `birthdate`, `birthdate_estimated`, `profile_image`, `primary_phone_number`, `primary_email_id`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `last_accessed_by`, `last_accessed_date`, `voided`, `voided_by`, `voided_date`, `voided_reason`, `uuid`) VALUES (2, 'admin', NULL, 'admin', NULL, NULL, 0, NULL, NULL, NULL, 1, '2016-01-01 00:00:00', NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL);
INSERT INTO `person` (`person_id`, `first_name`, `middle_name`, `last_name`, `gender`, `birthdate`, `birthdate_estimated`, `profile_image`, `primary_phone_number`, `primary_email_id`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `last_accessed_by`, `last_accessed_date`, `voided`, `voided_by`, `voided_date`, `voided_reason`, `uuid`) VALUES (3, 'Akash', NULL, 'Jain', 'M', '1992-03-05', 0, NULL, '+91 9880122075', 'akashjain@live.com', 2, '2016-01-01 00:00:00', NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL);
END if;
END $$
DELIMITER ;
call personProc();
COMMIT;


-- -----------------------------------------------------
-- Data for table `users`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `usersProc`;
DELIMITER $$
CREATE PROCEDURE `usersProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `users` into myId;
if(myId = 0)
then
INSERT INTO `users` (`user_id`, `system_id`, `person_id`, `username`, `password`, `secret_question`, `secret_answer`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `retired`, `retired_by`, `retired_date`, `retired_reason`, `uuid`) VALUES (1, 'system', 1, 'system', '$2a$10$M/XgFHCMAZJN5hHwFIMezBZkb/RtRPUhDHfIH6H3UNk/9pSN9hfTm', NULL, NULL, NULL, '2016-01-01 00:00:00', NULL, NULL, DEFAULT, NULL, NULL, NULL, NULL);
INSERT INTO `users` (`user_id`, `system_id`, `person_id`, `username`, `password`, `secret_question`, `secret_answer`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `retired`, `retired_by`, `retired_date`, `retired_reason`, `uuid`) VALUES (2, 'admin', 2, 'admin', '$2a$10$DJmnpncRBOw/3VS12HL3.OgupbRtRPUhDHfFiHJWVyVWk5kmNBfUW', NULL, NULL, 1, '2016-01-01 00:00:00', NULL, NULL, DEFAULT, NULL, NULL, NULL, NULL);
INSERT INTO `users` (`user_id`, `system_id`, `person_id`, `username`, `password`, `secret_question`, `secret_answer`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `retired`, `retired_by`, `retired_date`, `retired_reason`, `uuid`) VALUES (3, 'operator1', 3, 'operator1', '$2a$10$Fgt.XgFHCMAZJN5hHwFIMezBZkb/P.r1H7hoyTQGqPXutVpnyiTFi', NULL, NULL, 1, '2016-01-01 00:00:00', NULL, NULL, DEFAULT, NULL, NULL, NULL, 'aa82fc56-f179-47c4-b0ab-ab45fe9b1e8f');
END if;
END $$
DELIMITER ;
call usersProc();
COMMIT;

START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `patientIdentifierTypeProc`;
DELIMITER $$
CREATE PROCEDURE `patientIdentifierTypeProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `patient_identifier_type` into myId;
if(myId = 0)
then
INSERT INTO `patient_identifier_type` (`patient_identifier_type_id`, `name`, `required`, `format`, `format_description`, `validator`, `description`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `retired`, `retired_by`, `retired_date`, `retired_reason`, `uuid`) VALUES (1, 'ivl_identifier_1', 1, '/^[a-z0-9]+_[a-z0-9]+$/', 'Must follow the pattern NNNNNNNN-N (up to 8 digits followed by a dash and a final digit)', NULL, 'ivl_identifier_1', 1, '2016-01-01 00:00:00', NULL, NULL, 0, NULL, NULL, NULL, '23191573-833a-4c96-9216-4135ddc3fe16');
END if;
END $$
DELIMITER ;
call patientIdentifierTypeProc();
COMMIT;

-- -----------------------------------------------------
-- Data for table `person_attribute_type`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `personAttributeTypeProc`;
DELIMITER $$
CREATE PROCEDURE `personAttributeTypeProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `person_attribute_type` into myId;
if(myId = 0)
then
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (1, 'BloodGroup', 'Blood group of this person', 'ee8da613-1645-46a7-af2b-ee663822bb4c');
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (2, 'Income', 'Current income of person', '89412a44-0a5d-4dbe-a4d6-7d818f9e7786');
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (3, 'Occupation', 'Occupation of person', '6a04f702-acc2-47be-b117-233cebdb3a55');
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (4, 'Landline', 'Landline contact number', 'b18f7115-6105-4a77-8ce4-95d8d46469c3');
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (5, 'Height', 'Height of person', 'b18f7115-6105-4a77-8ce4-95d8d46469c4');
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (6, 'Weight', 'Weight of person', 'b18f7115-6105-4a77-8ce4-95d8d46469c7');
INSERT INTO `person_attribute_type` (`person_attribute_type_id`, `name`, `description`, `uuid`) VALUES (7, 'Comments', 'Note for this person', 'b18f7115-6105-4a77-8ce4-95d8d46469c8');
END if;
END $$
DELIMITER ;
call personAttributeTypeProc();
COMMIT;

-- -----------------------------------------------------
-- Data for table `machine`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `machineProc`;
DELIMITER $$
CREATE PROCEDURE `machineProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `machine` into myId;
if(myId = 0)
then
INSERT INTO `machine` (`machine_id`, `name`, `description`, `serial_number`, `model_number`, `created_by`, `created_date`, `last_modified_by`, `last_modified_date`, `retired`, `retired_by`, `retired_date`, `retired_reason`, `uuid`) VALUES (1, 'Eye Fundus Camera 1', 'Intuvision labs eye fundus camera. For more http:\\\\www.intuvisionlabs.com', 'IVL-FC1-1000', 'FC1', 2, '2016-01-01 00:00:00', NULL, NULL, DEFAULT, NULL, NULL, NULL, NULL);
END if;
END $$
DELIMITER ;
call machineProc();
COMMIT;

-- -----------------------------------------------------
-- Data for table `role`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `roleProc`;
DELIMITER $$
CREATE PROCEDURE `roleProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `role` into myId;
if(myId = 0)
then
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('SYSTEM', 'Root user of MRS system .. have additional access to change fundamental structure of the database model.', '8d94f852-c2cc-11de-8d13-0010c6dffd0f');
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('ADMIN', 'Administrator of system.', '52139f42-e378-448f-9cbe-8ce41fed89a6');
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('OPERATOR', 'Primary user of system.', '338e0450-3280-4cbd-a434-c71b48c140ea');
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('DOCTOR', 'A specialist.', 'd09329d4-480a-4d6b-beef-8ee7f3e94bb8');
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('Anonymous', 'Privileges for non-authenticated users.', '774b2af3-6437-4e5a-a310-547554c7c65c');
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('Authenticated', 'Privileges gained once authentication has been established.', 'f7fd42ef-880e-40c5-972d-e4ae7c990de2');
INSERT INTO `role` (`role_id`, `description`, `uuid`) VALUES ('Provider', 'All users with the \'Provider\' role will appear as options in the default Infopath.', '8d94f280-c2cc-11de-8d13-0010c6dffd0f');
END if;
END $$
DELIMITER ;
call roleProc();
COMMIT;


-- -----------------------------------------------------
-- Data for table `privilege`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `privilegeProc`;
DELIMITER $$
CREATE PROCEDURE `privilegeProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `privilege` into myId;
if(myId = 0)
then
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Observations', 'Able to add patient observations', '533be202-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Patient Identifiers', 'Able to add patient identifiers', '533be33a-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Patient', 'Able to add patients', '533be474-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Person', 'Able to add person objects', '533be509-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add User', 'Able to add users', '533be6d8-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Visit', 'Able to add visits', 'ee7f85d2-1048-4206-b562-8c0742871683');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Notes', 'Able to delete patient notes', '90c80c12-8c66-4adf-a31c-2439f39cd530');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Observation', 'Able to delete patient observations', '533be894-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Patient Identifiers', 'Able to delete patient identifiers', '533be971-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Patient', 'Able to delete patients', '533bea57-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Person', 'Able to delete objects', '533beac6-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete User', 'Able to delete users', '533bec83-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Visit', 'Able to delete visits', '0bd935bf-5b14-4c46-9c0f-3839a80c4c8e');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Notes', 'Able to edit patient notes', '2144fb04-aac0-4db5-8411-0b649dafaa06');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Observations', 'Able to edit patient observations', '533bee3c-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Organization', 'Able to edit organization', '3549f7bf-4b47-4e0d-8d96-7dcd57a24b5f');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Patient Identifiers', 'Able to edit patient identifiers', '533bef17-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Patient', 'Able to edit patients', '533beff8-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Person', 'Able to edit person objects', '533bf065-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit User', 'Able to edit users', '533bf28a-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Visit', 'Able to edit visits', '7f764afc-3b39-4591-8063-2a4314e69dd8');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Get Machines', 'Able to get machines', '016f28dd-fbdc-46c5-9247-609cf0afabb0');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Get Notes', 'Able to get patient notes', '9dae876f-abd9-4e1b-968a-ca9daf62dbf0');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Observation', 'Able to get patient observations', 'd05118c6-2490-4d78-a41a-390e3596a245');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Get Organization', 'Able to get organization', 'b51905de-ba9c-4b77-9149-e63fc23b0f69');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Get Patient Identifiers', 'Able to get patient identifiers', 'd05118c6-2490-4d78-a41a-390e3596a243');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Patient', 'Able to get patients', 'd05118c6-2490-4d78-a41a-390e3596a244');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Get Roles', 'Able to get user roles', 'd05118c6-2490-4d78-a41a-390e3596a235');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List User', 'Able to get users', 'd05118c6-2490-4d78-a41a-390e3596a249');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Visit', 'Able to get visits', 'd05118c6-2490-4d78-a41a-390e3596a214');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Person', 'Able to get person objects', 'd05118c6-2490-4d78-a41a-390e3596a224');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Manage Machines', 'Able to add/edit/delete machines', 'de0a7f23-84a4-4e66-9045-49d22b6a61b8');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Manage Roles', 'Able to add/edit/delete user roles', '533bfb8e-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Person', 'Able to purge person objects', '88d55722-6ad6-4d47-b5d0-32d8c28fa7e1');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Machines', 'Able to view machines', 'fbfe5511-9bfb-4994-8910-f71214cba967');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Observation', 'Able to view patient observations', '533c09e3-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Patient Identifiers', 'Able to view patient identifiers', '533c0b9c-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Patient', 'Able to view patients', '533c0c7e-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Person', 'Able to view person objects', '533c0cf0-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Roles', 'Able to view user roles', '533c10e5-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View User', 'Able to view users', '533c11ca-b9d5-11e5-9fa4-dac85c732c84');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Patient', 'Able to purge patient objects', 'e17086d7-9c0f-493b-9a7f-f8ae4e7ccac5');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Visit', 'Able to view patients visits', 'ea60d5a9-146a-451a-a67f-d912f5f5d320');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Visit', 'Able to purge visit objects', '9379b386-75a8-4abc-8695-03eaeea1cfe0');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Person Attribute Type', 'Able to get person attribute types', '72afccd6-2cf3-490f-833d-8fd084f29693');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Patient Identifier Type', 'Able to get patient identifier types', '2bd378dd-3ed5-463f-95ef-d29091e060b1');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Patient Identifier Type', 'Able to view patient identifier types', 'f50e85c3-060e-4509-b103-d450914ae756');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge User', 'Able to purge user objects', '4acba53e-da51-4910-ac3c-f15661ace749');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Report', 'Able to get reports', '49c42dae-7dce-4418-bd71-b76803c4a57e');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Report', 'Able to view reports', '2b6d8463-e30a-47f4-9ae8-f9af89050bb4');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Machine', 'Able to get machines', '96e3ad7d-c015-47d7-98f3-b779ad7dc384');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Machine', 'Able to view machine', 'b70713f4-e954-44f0-961d-b1a533b54f3b');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('List Eye Fundus Image Annotation', 'Able to get annotations', '76f3adc4-195c-4753-bf8f-bdd7e5dcfa5c');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('View Eye Fundus Image Annotation', 'Able to view annotations', '7f6a84d2-e5b8-464c-a6ee-9954e1ee2e93');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Patient Identifier Type', 'Able to delete patient identifier types', '1f508ff0-d3e0-48ce-94c3-1407d5362279');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Patient Identifier Type', 'Able to purge patient identifier types', 'efdcd9bf-163d-41f3-a55c-96ebb61ab9a7');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Observation', 'Able to purge patient observations', '27c9e267-add9-4940-a272-fffde1ac8bdb');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Eye Fundus Image Annotation', 'Able to delete annotations', '27761b64-409a-4d0b-b142-3043cc4f1c12');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Eye Fundus Image Annotation', 'Able to purge annotations', 'c23ac3cc-ae5d-4089-8684-ec10e368da27');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Report', 'Able to delete reports', '536ecac1-7c77-44a9-896e-ecd0ba581f7a');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Report', 'Able to purge reports', 'a7ca3791-f79e-4139-b854-5177e9f5a60b');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Machine', 'Able to add machine', '36fcebe6-ee42-45d1-90c4-088a65010a62');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Edit Machine', 'Able to edit machine', 'ea0bc2a5-16db-468d-8686-3402d4e6e7f7');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Delete Machine', 'Able to delete machine', 'f2196a3e-fc34-457a-920f-1d08ed51d578');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Purge Machine', 'Able to purge machine', '6aa6d585-925d-4d22-8a5a-81dcf5826e50');
INSERT INTO `privilege` (`privilege_id`, `description`, `uuid`) VALUES ('Add Report', 'Able to add report', 'c8298848-a786-4ed3-88f5-61556b58ee61');
END if;
END $$
DELIMITER ;
call privilegeProc();
COMMIT;

-- -----------------------------------------------------
-- Data for table `role_privilege`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `rolePrivilegeProc`;
DELIMITER $$
CREATE PROCEDURE `rolePrivilegeProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `role_privilege` into myId;
if(myId = 0)
then
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Add Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Edit Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Person Attribute Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Add Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Edit Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Patient Identifier Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Patient Identifier Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Patient Identifier Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Patient Identifier Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Add Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Edit Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Add User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Edit User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'List Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'View Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Add Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Edit Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Delete Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('ADMIN', 'Purge Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Add Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Edit Person');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Person Attribute Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Add Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Edit Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Delete Patient');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Patient Identifier Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Patient Identifier Type');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Add Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Edit Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Delete Visit');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Delete Observation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Delete Eye Fundus Image Annotation');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View User');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Delete Report');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'List Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'View Machine');
INSERT INTO `role_privilege` (`role_id`, `privilege_id`) VALUES ('OPERATOR', 'Add Report');
END if;
END $$
DELIMITER ;
call rolePrivilegeProc();
COMMIT;

-- -----------------------------------------------------
-- Data for table `user_role`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `userRoleProc`;
DELIMITER $$
CREATE PROCEDURE `userRoleProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `user_role` into myId;
if(myId = 0)
then
INSERT INTO `user_role` (`user_id`, `role_id`) VALUES (1, 'SYSTEM');
INSERT INTO `user_role` (`user_id`, `role_id`) VALUES (2, 'ADMIN');
INSERT INTO `user_role` (`user_id`, `role_id`) VALUES (3, 'OPERATOR');
END if;
END $$
DELIMITER ;
call userRoleProc();
COMMIT;


-- -----------------------------------------------------
-- Data for table `user_property`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `userpropertyProc`;
DELIMITER $$
CREATE PROCEDURE `userpropertyProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `user_property` into myId;
if(myId = 0)
then
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (2, 'locales', 'en');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (2, 'shortDateFormat', 'dd-MM-yyyy');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (2, 'longDateFormat', 'dd MMMM yyyy');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (2, 'shortTimeFormat', 'hh:mm a');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (2, 'longTimeFormat', 'hh:mm:ss a');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (3, 'locales', 'en');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (3, 'shortDateFormat', 'dd-MM-yyyy');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (3, 'longDateFormat', 'dd MMMM yyyy');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (3, 'shortTimeFormat', 'hh:mm a');
INSERT INTO `user_property` (`user_id`, `property_id`, `value`) VALUES (3, 'longTimeFormat', 'hh:mm:ss a');
END if;
END $$
DELIMITER ;
call userpropertyProc();
COMMIT;


-- -----------------------------------------------------
-- Data for table `concept_set`
-- -----------------------------------------------------
START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `conceptSetProc`;
DELIMITER $$
CREATE PROCEDURE `conceptSetProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `concept_set` into myId;
if(myId = 0)
then
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (1, 4, 3, 0, '95b46604-59f4-4543-927e-bafad448ae33');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (2, 9, 5, 0, '67a6bc43-a542-4115-b3b5-ee14088fa881');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (3, 9, 6, 1, 'f5b49534-b12d-49f2-98fe-d9bd82257faa');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (4, 9, 7, 2, '26f61c78-1ff0-4647-a78e-861cd8d70a77');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (5, 10, 5, 0, 'caa9adce-a8c2-4e69-9bfb-5d38f9f3b6df');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (6, 10, 6, 1, 'badd7d98-17a2-48b4-9594-653647657764');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (7, 10, 7, 2, 'db22ee07-cf33-4319-ba54-0adf9072b642');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (8, 11, 9, 0, '685e8de7-0e9d-44d1-bdfb-db73753b31a5');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (9, 11, 10, 1, 'bcfd5993-961a-4ae1-a418-45f8ce74be2a');
INSERT INTO `concept_set` (`concept_set_id`, `aggregator_concept_id`, `member_concept_id`, `sort_weight`, `uuid`) VALUES (10, 11, 8, 2, 'adfb15f2-30d7-47df-a94a-5097cf155e73');
END if;
END $$
DELIMITER ;
call conceptSetProc();
COMMIT;

START TRANSACTION;
USE `dbName`;
DROP PROCEDURE IF EXISTS `reportTypeProc`;
DELIMITER $$
CREATE PROCEDURE `reportTypeProc` ()
BEGIN
DECLARE myId INT default 0;
SELECT COUNT(*) FROM `report_type` into myId;
if(myId = 0)
then
INSERT INTO `report_type` (`report_type_id`, `name`, `description`, `uuid`) VALUES (1, 'Fundus Image Report', 'Report for fundus image.', 'd38849b7-6ee9-43a4-8a1d-9d27fdae8656');
INSERT INTO `report_type` (`report_type_id`, `name`, `description`, `uuid`) VALUES (2, 'Annotation Report', 'Report for annotation.', '27dac695-d6f3-41c1-87e0-d525b07bbcc7');
INSERT INTO `report_type` (`report_type_id`, `name`, `description`, `uuid`) VALUES (3, 'Glaucoma Analysis Report', 'Report for CDR.', 'b09445ba-2838-49cd-970a-f9cc0c965a82');
END if;
END $$
DELIMITER ;
call reportTypeProc();
COMMIT;

SET FOREIGN_KEY_CHECKS=1;
