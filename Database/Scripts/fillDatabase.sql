USE onsightdb;

--Add person types
INSERT INTO PersonType (type)
VALUES ('master'), ('admin'), ('supervisor'), ('contractor'), ('subcontractor');

--Add all countries
INSERT INTO Country (countryName) VALUES
('Canada'),
('United States');

--Add all provinces
INSERT INTO Province (provinceName) VALUES
 ('Alaska'),
 ('Alabama'),
 ('American Samoa'),
 ('Arizona'),
 ('Arkansas'),
 ('California'),
 ('Colorado'),
 ('Connecticut'),
 ('Delaware'),
 ('District of Columbia'),
 ('Florida'),
 ('Georgia'), 
 ('Guam'),
 ('Hawaii'),
 ('Idaho'),
 ('Illinois'),
 ('Indiana'),
 ('Iowa'),
 ('Kansas'),
 ('Kentucky'),
 ('Louisiana'),
 ('Maine'),
 ('Maryland'),
 ('Massachusetts'),
 ('Michigan'),
 ('Minnesota'),
 ('Mississippi'),
 ('Missouri'),
 ('Montana'),
 ('Nebraska'),
 ('Nevada'),
 ('New Hampshire'),
 ('New Jersey'),
 ('New Mexico'), 
 ('New York'),
 ('North Carolina'),
 ('North Dakota'),
 ('Northern Mariana Islands'),
 ('Ohio'),
 ('Oklahoma'),
 ('Oregon'),
 ('Palau'), 
 ('Pennsylvania'),
 ('Puerto Rico'),
 ('Rhode Island'),
 ('South Carolina'),
 ('South Dakota'),
 ('Tennessee'),
 ('Texas'), 
 ('Utah'),
 ('Vermont'),
 ('Virgin Islands'),
 ('Virginia'),
 ('Washington'),
 ('West Virginia'),
 ('Wisconsin'), 
 ('Wyoming'),
 ('Alberta'),
 ('British Columbia'),
 ('Manitoba'),
 ('New Brunswick'),
 ('Newfoundland and Labrador'),
 ('Northwest Territories'),
 ('Nova Scotia'),
 ('Nunavut'),
 ('Ontario'),
 ('Prince Edward Island'),
 ('Québec'),
 ('Saskatchewan'),
 ('Yukon Territory');

--Add all companies
INSERT INTO Company (companyName, contactNumber, contactEmail, address, postalCode, city, country, province)
VALUES ('Air Postal', '5197325243', 'customer_support@airpostal.ca', '4615 Ontario St.', 'T0M1T1', 'Montreal', 1, 68),
('GeoLive', '2263034284', 'GeoLiveSupport@hotmail.com', '5120 Homestead Drive', '33801', 'Lakeland', 2, 11);

--Add all users

--Add all masters
INSERT INTO Person (firstName, lastName, city, address, postalCode, phone, email, password, online, type, country, province)
VALUES ('Timothy', 'Andrews', 'Montreal', '153 Townshill st.', 'G2F4D6', '5196325723', 'TAndrews@gmail.ca', 'brissles', 1, 1, 1, 68),
('Meghan', 'Crowsby', 'Montreal', '38 Riverdale st.', 'P6D5F2', '5197564894', 'MeghanCrows@hotmail.com', 'meghanxo1', 1, 1, 1, 68);

--Add all admins
INSERT INTO Person (firstName, lastName, city, address, postalCode, phone, email, password, online, type, company, country, province)
VALUES ('Brian', 'Hemsworth', 'Montreal', '273 Rierdan Lane', 'H8G8D2', '5197451845', 'bworth@gmail.ca', 'hemmie4life', 1, 2, 1, 1, 68),
('Brett', 'Daniels', 'Montreal', '118 Forksville Drive', 'T7D2M9', '5197389461', 'JackDaniels@hotmail.com', 'Jack1Liter', 1, 2, 1, 1, 68);

--Add all supervisors
INSERT INTO Person (firstName, lastName, city, address, postalCode, phone, email, password, online, type, company, country, province)
VALUES ('Richard', 'Reynolds', 'Montreal', '857 Goodville Lane', 'Z4S7N3', '5195018261', 'RickyRoll@gmail.ca', 'R0LL3RLIF3', 1, 3, 1, 1, 68),
('Meyer', 'Wilson', 'Montreal', '3B 138 Crestview cr.', 'U3M9X1', '5198279162', 'milson@outlook.com', 'Willie33', 1, 3, 1, 1, 68);

--Add all contractors
INSERT INTO Person (firstName, lastName, city, address, postalCode, phone, email, password, online, type, company, country, province)
VALUES ('Emily', 'Molson', 'Montreal', '534 Fullson av.', 'T1X6S9', '5193487012', 'MolsonLife@gmail.ca', 'MotorheadMolson1', 1, 4, 1, 1, 68),
('George', 'Gregory', 'Montreal', '893 Bristles Cr.', 'Q3Z5V2', '5197329724', 'gregorygeorge@hotmail.com', 'GregTheRag', 1, 4, 1, 1, 68),
('Tyler', 'Hickson', 'Montreal', '129 Perch Dr.', 'C7K3M7', '5196478634', 'HickTie@gmail.ca', 'hity1', 1, 5, 1, 1, 68),
('Chris', 'Brockman', 'Montreal', '824 Wilks Rd.', 'Y4B6G2', '5192479473', 'RockBrock@outlook.com', 'ChrisPratt3', 1, 5, 1, 1, 68);

--Add assigned subcontractors
INSERT INTO AssignedSubContractor(contractor, subContractor)
VALUES (7, 9), (7, 10);