SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

CREATE SCHEMA IF NOT EXISTS `university` DEFAULT CHARACTER SET utf8 ;
USE `university` ;

-- -----------------------------------------------------
-- Table `university`.`faculties`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `university`.`faculties` (
  `FacultyId` INT(11) NOT NULL ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`FacultyId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `university`.`departments`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `university`.`departments` (
  `DepartmentId` INT(11) NOT NULL ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  `faculties_FacultyId` INT(11) NULL ,
  PRIMARY KEY (`DepartmentId`) ,
  INDEX `fk_departments_faculties_idx` (`faculties_FacultyId` ASC) ,
  CONSTRAINT `fk_departments_faculties`
    FOREIGN KEY (`faculties_FacultyId` )
    REFERENCES `university`.`faculties` (`FacultyId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `university`.`courses`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `university`.`courses` (
  `CourseId` INT(11) NOT NULL ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  `departments_DepartmentId` INT(11) NULL ,
  PRIMARY KEY (`CourseId`) ,
  INDEX `fk_courses_departments1_idx` (`departments_DepartmentId` ASC) ,
  CONSTRAINT `fk_courses_departments1`
    FOREIGN KEY (`departments_DepartmentId` )
    REFERENCES `university`.`departments` (`DepartmentId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `university`.`titles`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `university`.`titles` (
  `TitleId` INT(11) NOT NULL ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`TitleId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `university`.`professors`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `university`.`professors` (
  `ProfessorId` INT(11) NOT NULL ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  `departments_DepartmentId` INT(11) NULL ,
  `titles_TitleId` INT(11) NULL ,
  PRIMARY KEY (`ProfessorId`) ,
  INDEX `fk_professors_departments1_idx` (`departments_DepartmentId` ASC) ,
  INDEX `fk_professors_titles1_idx` (`titles_TitleId` ASC) ,
  CONSTRAINT `fk_professors_departments1`
    FOREIGN KEY (`departments_DepartmentId` )
    REFERENCES `university`.`departments` (`DepartmentId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_professors_titles1`
    FOREIGN KEY (`titles_TitleId` )
    REFERENCES `university`.`titles` (`TitleId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `university`.`students`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `university`.`students` (
  `StudentId` INT(11) NOT NULL ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  `departments_DepartmentId` INT(11) NULL ,
  `courses_CourseId` INT(11) NULL ,
  PRIMARY KEY (`StudentId`) ,
  INDEX `fk_students_departments1_idx` (`departments_DepartmentId` ASC) ,
  INDEX `fk_students_courses1_idx` (`courses_CourseId` ASC) ,
  CONSTRAINT `fk_students_departments1`
    FOREIGN KEY (`departments_DepartmentId` )
    REFERENCES `university`.`departments` (`DepartmentId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_students_courses1`
    FOREIGN KEY (`courses_CourseId` )
    REFERENCES `university`.`courses` (`CourseId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

USE `university` ;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
