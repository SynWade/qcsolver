CREATE DATABASE onsightdb;

USE onsightdb;
/*
DROP TABLE AssignedSubContractor;
DROP TABLE Schedule;
DROP TABLE Certificate;
DROP TABLE License;
DROP TABLE Timestamp;
DROP TABLE TaskDetail;
DROP TABLE AssignedWorker;
DROP TABLE ConstructionSite;
DROP TABLE Person;
DROP TABLE Company;
DROP TABLE Country;
DROP TABLE Province;
DROP TABLE PersonType;
*/

CREATE TABLE PersonType
(
	personTypeId INT IDENTITY (1, 1) NOT NULL,
	type VARCHAR(25) NOT NULL,
    PRIMARY KEY CLUSTERED (personTypeId ASC)
);

CREATE TABLE Province
(
	provinceId INT IDENTITY (1, 1) NOT NULL,
	provinceName VARCHAR(25) NOT NULL,
    PRIMARY KEY CLUSTERED (provinceId ASC)
);

CREATE TABLE Country
(
	countryId INT IDENTITY (1, 1) NOT NULL,
	countryName VARCHAR(25) NOT NULL,
    PRIMARY KEY CLUSTERED (countryId ASC)
);

CREATE TABLE Company
(
  companyId INT IDENTITY (1, 1) NOT NULL,
	companyName VARCHAR(25) NOT NULL,
	contactNumber VARCHAR(10) NOT NULL,
	contactEmail VARCHAR(50) NOT NULL,
	address VARCHAR(25) NOT NULL,
	postalCode VARCHAR(6) NOT NULL,
	city VARCHAR(25) NOT NULL,
  country INT NOT NULL,
  province INT NOT NULL,
  PRIMARY KEY CLUSTERED (companyId ASC),
  FOREIGN KEY (country) REFERENCES Country(countryId),
  FOREIGN KEY (province) REFERENCES Province(provinceId)
);

CREATE TABLE Person
(
	personId INT IDENTITY (1, 1) NOT NULL,
	firstName VARCHAR(25) NOT NULL,
	lastName VARCHAR(25) NOT NULL,
  city VARCHAR(25) NOT NULL,
	address VARCHAR(25) NOT NULL,
	pictureLocation VARCHAR(255) NULL,
	contractLocation VARCHAR(255) NULL,
	postalCode VARCHAR(6) NOT NULL,
	phone VARCHAR(10) NOT NULL,
	email VARCHAR(50) NOT NULL,
	password VARCHAR(50) NOT NULL,
	online BIT NOT NULL,
	type INT NOT NULL,
  company INT NULL,
  country INT NOT NULL,
  province INT NOT NULL,
  PRIMARY KEY CLUSTERED (personId ASC),
  FOREIGN KEY (type) REFERENCES PersonType(personTypeId),
  FOREIGN KEY (company) REFERENCES Company(companyId),
  FOREIGN KEY (country) REFERENCES Country(countryId),
  FOREIGN KEY (province) REFERENCES Province(provinceId)
);

CREATE TABLE AssignedSubContractor
(
	assignedSubId INT IDENTITY (1, 1) NOT NULL,
	contractor INT NOT NULL,
	subContractor INT NOT NULL,
	PRIMARY KEY CLUSTERED (assignedSubId ASC),
	FOREIGN KEY (contractor) REFERENCES Person(personId),
	FOREIGN KEY (subContractor) REFERENCES Person(personId)		
);

CREATE TABLE ConstructionSite 
(
  constructionSiteId INT IDENTITY (1, 1) NOT NULL,
  address VARCHAR(25) NOT NULL,
  startDate DATE NOT NULL,
  endDate DATE NULL,
  city VARCHAR(25) NOT NULL,
  company INT NOT NULL,
  country INT NOT NULL,
  province INT NOT NULL,
  PRIMARY KEY CLUSTERED (constructionSiteId ASC),
  FOREIGN KEY (company) REFERENCES Company (companyId),
  FOREIGN KEY (country) REFERENCES Country (countryId),
  FOREIGN KEY (province) REFERENCES Province (provinceId)
);

CREATE TABLE Timestamp
(
	timestampId INT IDENTITY (1, 1) NOT NULL,
	timeIn datetime NOT NULL,
	timeOut datetime NULL,
	person INT NOT NULL,
	constructionSite INT NOT NULL,
	PRIMARY KEY CLUSTERED (timestampId ASC),
	FOREIGN KEY (person) REFERENCES Person (personId),
	FOREIGN KEY (constructionSite) REFERENCES ConstructionSite (constructionSiteId)
);
CREATE TABLE AssignedWorker
(
	assignedId INT IDENTITY (1, 1) NOT NULL,
	asignDate DATE NOT NULL,
	unassignDate DATE NULL,
	constructionSite INT NOT NULL,
  person INT NOT NULL,
  PRIMARY KEY CLUSTERED (assignedId ASC),
  FOREIGN KEY (constructionSite) REFERENCES ConstructionSite(constructionSiteId),
  FOREIGN KEY (person) REFERENCES Person(personId)
);

CREATE TABLE TaskDetail
(
	detailId INT IDENTITY (1, 1) NOT NULL,
	details VARCHAR(255) NOT NULL,
  assigned INT NOT NULL,
  PRIMARY KEY CLUSTERED (detailId ASC),
  FOREIGN KEY (assigned) REFERENCES AssignedWorker(assignedId)
);

CREATE TABLE License
(
	licenseId INT IDENTITY (1, 1) NOT NULL,
	licenseName VARCHAR(25) NOT NULL,
	dateIssued DATE NOT NULL,
	expirationDate DATE NULL,
	fileLocation VARCHAR(255) NOT NULL,
	person INT NOT NULL,
  PRIMARY KEY CLUSTERED (licenseId ASC),
  FOREIGN KEY (person) REFERENCES Person(personId)
);

CREATE TABLE Certificate
(
	certificateId INT IDENTITY (1, 1) NOT NULL,
	certificateName VARCHAR(25) NOT NULL,
	dateIssued DATE NOT NULL,
	fileLocation VARCHAR(255) NOT NULL,
	person INT NOT NULL,
  PRIMARY KEY CLUSTERED (certificateId ASC),
  FOREIGN KEY (person) REFERENCES Person(personId)
);

CREATE TABLE Schedule
(
	scheduleId INT IDENTITY (1, 1) NOT NULL,
	startDateTime datetime NOT NULL,
	duration INT NOT NULL,
	person INT NOT NULL,
	constructionSite INT NOT NULL,
  PRIMARY KEY CLUSTERED (scheduleId ASC),
  FOREIGN KEY (person) REFERENCES Person(personId),
  FOREIGN KEY (constructionSite) REFERENCES ConstructionSite(constructionSiteId)	
);