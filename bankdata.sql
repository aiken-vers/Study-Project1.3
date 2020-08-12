-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: bankdata
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cards`
--

DROP TABLE IF EXISTS `cards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cards` (
  `iddeposits` int(11) NOT NULL AUTO_INCREMENT,
  `idclient` int(11) NOT NULL,
  `type` varchar(45) NOT NULL,
  `pincode` varchar(4) DEFAULT NULL,
  `cvv2_code` varchar(3) DEFAULT NULL,
  `duration` date NOT NULL,
  `balance` decimal(15,2) DEFAULT '0.00',
  PRIMARY KEY (`iddeposits`),
  UNIQUE KEY `iddeposits_UNIQUE` (`iddeposits`),
  KEY `idclient_idx` (`idclient`),
  CONSTRAINT `Cidclient` FOREIGN KEY (`idclient`) REFERENCES `clients` (`idclients`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=62120006 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cards`
--

LOCK TABLES `cards` WRITE;
/*!40000 ALTER TABLE `cards` DISABLE KEYS */;
INSERT INTO `cards` VALUES (62120001,4,'кредитная','3236','091','2020-05-22',5600.00),(62120003,5,'дебетовая','6498','978','2020-05-22',21000.00),(62120004,5,'предоплаченная','9402','767','2020-05-22',2989500.00),(62120005,4,'предоплаченная','0426','295','2020-05-24',14182.00);
/*!40000 ALTER TABLE `cards` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cards_BEFORE_INSERT` BEFORE INSERT ON `cards` FOR EACH ROW BEGIN
SET NEW.pincode = LPAD((SELECT ROUND(RAND()*10000)), 4, '0');
SET NEW.cvv2_code=LPAD((SELECT ROUND(RAND()*1000)), 3, '0');
-- IF NEW.iddeposits<1000
-- THEN
-- SET NEW.iddeposits=concat(6212,LPAD(NEW.iddeposits, 4, '0'));
-- END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `cards_AFTER_INSERT` AFTER INSERT ON `cards` FOR EACH ROW BEGIN
insert into journal(idclient, event, idevent, balance) values (NEW.idclient, 'Заведена карта',NEW.iddeposits,NEW.balance);
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `clients`
--

DROP TABLE IF EXISTS `clients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `clients` (
  `idclients` int(11) NOT NULL AUTO_INCREMENT,
  `FIO` varchar(60) NOT NULL,
  `passportid` int(11) NOT NULL,
  `Address` varchar(60) DEFAULT NULL,
  `number` int(11) DEFAULT NULL,
  `email` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`idclients`),
  UNIQUE KEY `idClients_UNIQUE` (`idclients`),
  UNIQUE KEY `passportid_UNIQUE` (`passportid`),
  UNIQUE KEY `number_UNIQUE` (`number`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clients`
--

LOCK TABLES `clients` WRITE;
/*!40000 ALTER TABLE `clients` DISABLE KEYS */;
INSERT INTO `clients` VALUES (4,'Подопытный 1',80120017,'New Home',101001,'dalexator@gmail.com'),(5,'Подопытный 2',141017,'Дикопольцева',101002,NULL),(8,'Подопытный 3',5165195,'',NULL,NULL),(11,'подопытный4',10145990,NULL,NULL,NULL);
/*!40000 ALTER TABLE `clients` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `clients_AFTER_DELETE` AFTER DELETE ON `clients` FOR EACH ROW BEGIN
DELETE FROM logins WHERE logins.idclient=old.idclients;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `deposits`
--

DROP TABLE IF EXISTS `deposits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `deposits` (
  `iddeposits` int(11) NOT NULL AUTO_INCREMENT,
  `idclient` int(11) NOT NULL,
  `type` varchar(45) NOT NULL,
  `balance` decimal(15,2) DEFAULT '0.00',
  `duration` date DEFAULT NULL,
  `refill` tinyint(1) DEFAULT '1',
  `cut` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`iddeposits`),
  UNIQUE KEY `iddeposits_UNIQUE` (`iddeposits`),
  KEY `idclient_idx` (`idclient`),
  CONSTRAINT `Didclient` FOREIGN KEY (`idclient`) REFERENCES `clients` (`idclients`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10120004 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `deposits`
--

LOCK TABLES `deposits` WRITE;
/*!40000 ALTER TABLE `deposits` DISABLE KEYS */;
INSERT INTO `deposits` VALUES (10120000,4,'расчётный',5100.00,'2022-05-22',1,1),(10120001,5,'кредитный',999500.00,'2022-05-22',0,1),(10120002,4,'накопительный',1.00,'2024-05-26',1,0),(10120003,4,'кредитный',100000.00,'2022-05-26',0,1);
/*!40000 ALTER TABLE `deposits` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `deposits_AFTER_INSERT` AFTER INSERT ON `deposits` FOR EACH ROW BEGIN
insert into journal(idclient, event, idevent, balance) values (NEW.idclient, 'Открыт вклад',NEW.iddeposits,NEW.balance);
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `journal`
--

DROP TABLE IF EXISTS `journal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `journal` (
  `idjournal` int(11) NOT NULL AUTO_INCREMENT,
  `idclient` int(11) NOT NULL,
  `event` varchar(120) DEFAULT NULL,
  `date` timestamp NULL DEFAULT NULL,
  `idevent` int(11) DEFAULT NULL,
  `balance` decimal(15,2) DEFAULT NULL,
  `operation` decimal(15,2) DEFAULT NULL,
  PRIMARY KEY (`idjournal`),
  UNIQUE KEY `idjournal_UNIQUE` (`idjournal`),
  KEY `idclient_idx` (`idclient`),
  CONSTRAINT `Jidclient` FOREIGN KEY (`idclient`) REFERENCES `clients` (`idclients`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=86 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `journal`
--

LOCK TABLES `journal` WRITE;
/*!40000 ALTER TABLE `journal` DISABLE KEYS */;
INSERT INTO `journal` VALUES (3,4,'Пользователь был зарегистрирован','2019-05-21 06:29:56',NULL,NULL,NULL),(4,4,'Заведена карта','2019-05-21 07:42:33',62120000,0.00,NULL),(5,4,'Заведена карта','2019-05-21 14:59:46',62120001,50000.00,NULL),(6,4,'Открыт вклад','2019-05-21 14:59:50',10120000,0.00,NULL),(7,4,'Внесение наличных','2019-05-21 15:01:25',62120001,50500.00,500.00),(8,4,'Внесение наличных','2019-05-21 15:01:44',62120001,50600.00,100.00),(9,4,'Внесение наличных','2019-05-21 15:01:47',10120000,100.00,100.00),(10,4,'Заведена карта','2019-05-21 15:01:54',62120002,0.00,NULL),(11,4,'Внесение наличных','2019-05-21 15:01:58',62120002,100.00,100.00),(12,4,'Снятие наличных','2019-05-21 15:02:07',62120001,45600.00,5000.00),(13,5,'Пользователь был зарегистрирован','2019-05-22 03:34:14',NULL,NULL,NULL),(14,5,'Заведена карта','2019-05-22 03:34:48',62120003,0.00,NULL),(15,5,'Заведена карта','2019-05-22 03:34:57',62120004,4000000.00,NULL),(16,5,'Открыт вклад','2019-05-22 03:35:06',10120001,1000000.00,NULL),(17,5,'Перевод наличных (списание)','2019-05-22 03:35:29',62120004,3990000.00,10000.00),(18,4,'Перевод наличных (пополнение)','2019-05-22 03:35:29',62120002,10100.00,10000.00),(19,5,'Перевод наличных (списание)','2019-05-22 03:35:42',62120004,2990000.00,1000000.00),(20,4,'Перевод наличных (пополнение)','2019-05-22 03:35:42',62120002,1010100.00,1000000.00),(21,5,'Снятие наличных','2019-05-22 03:36:10',10120001,999500.00,500.00),(22,4,'Внесение наличных','2019-05-22 03:36:44',10120000,5100.00,5000.00),(25,5,'Снятие наличных','2019-05-22 14:23:04',62120004,2989500.00,500.00),(26,5,'Внесение наличных','2019-05-22 14:23:07',62120003,500.00,500.00),(27,5,'Внесение наличных','2019-05-22 14:23:09',62120003,1000.00,500.00),(28,4,'Пользователь вошёл в систему','2019-05-24 02:08:01',NULL,NULL,NULL),(29,4,'Заведена карта','2019-05-24 02:09:14',62120005,9182.00,NULL),(30,8,'Пользователь был зарегистрирован','2019-05-24 02:11:37',NULL,NULL,NULL),(31,4,'Пользователь вошёл в систему','2019-05-24 02:15:14',NULL,NULL,NULL),(32,4,'Пользователь вошёл в систему','2019-05-24 02:25:47',NULL,NULL,NULL),(33,4,'Пользователь вошёл в систему','2019-05-24 02:26:31',NULL,NULL,NULL),(34,4,'Пользователь вошёл в систему','2019-05-24 02:28:57',NULL,NULL,NULL),(39,4,'Пользователь вошёл в систему','2019-05-26 04:35:16',NULL,NULL,NULL),(40,4,'Пользователь вошёл в систему','2019-05-26 04:37:50',NULL,NULL,NULL),(41,4,'Пользователь вошёл в систему','2019-05-26 04:48:24',NULL,NULL,NULL),(42,4,'Пользователь вошёл в систему','2019-05-26 04:52:27',NULL,NULL,NULL),(43,4,'Пользователь вошёл в систему','2019-05-26 04:54:33',NULL,NULL,NULL),(44,4,'Снятие наличных','2019-05-26 04:55:02',62120001,40600.00,5000.00),(45,4,'Снятие наличных','2019-05-26 04:55:05',62120001,35600.00,5000.00),(46,4,'Пользователь вошёл в систему','2019-05-26 04:56:28',NULL,NULL,NULL),(47,4,'Пользователь вошёл в систему','2019-05-26 04:57:27',NULL,NULL,NULL),(48,4,'Пользователь вошёл в систему','2019-05-26 04:59:08',NULL,NULL,NULL),(49,4,'Пользователь вошёл в систему','2019-05-26 05:00:02',NULL,NULL,NULL),(50,4,'Пользователь вошёл в систему','2019-05-26 05:20:59',NULL,NULL,NULL),(51,4,'Пользователь вошёл в систему','2019-05-26 05:30:06',NULL,NULL,NULL),(52,4,'Пользователь вошёл в систему','2019-05-26 05:46:13',NULL,NULL,NULL),(53,4,'Пользователь вошёл в систему','2019-05-26 05:48:22',NULL,NULL,NULL);
/*!40000 ALTER TABLE `journal` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `journal_BEFORE_INSERT` BEFORE INSERT ON `journal` FOR EACH ROW BEGIN
SET NEW.date = (SELECT NOW());
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `logins`
--

DROP TABLE IF EXISTS `logins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `logins` (
  `login` varchar(12) NOT NULL,
  `password` varchar(255) NOT NULL,
  `idclient` int(11) DEFAULT NULL,
  `idworker` int(11) DEFAULT NULL,
  `idface` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`login`),
  UNIQUE KEY `login_UNIQUE` (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `logins`
--

LOCK TABLES `logins` WRITE;
/*!40000 ALTER TABLE `logins` DISABLE KEYS */;
INSERT INTO `logins` VALUES ('admin','hacked',NULL,1,NULL),('admin2','C84258E9C39059A89AB77D846DDAB909',NULL,5,NULL),('bank-admin2','8640EAD7452CED4DFDA8362EDFC5B1BA',NULL,6,NULL),('hacker','C84258E9C39059A89AB77D846DDAB909',NULL,NULL,NULL),('user1','24C9E15E52AFC47C225B757E7BEE1F9D',4,NULL,NULL),('user2','7E58D63B60197CEB55A1C487989A3720',5,NULL,NULL),('user3','92877AF70A45FD6A2ED7FE81E1236B78',8,NULL,NULL),('user4','password4',11,NULL,NULL);
/*!40000 ALTER TABLE `logins` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `logins_before_insert_all` BEFORE INSERT ON `logins` FOR EACH ROW BEGIN
IF CHAR_LENGTH(NEW.login) > 12 THEN
	SIGNAL sqlstate '45000'
		SET message_text='Введённый логин превышает максимальную длину (12 символов)';
END IF;
IF NEW.idclient IS NOT NULL THEN
	IF (EXISTS(SELECT 1 FROM logins WHERE idclient = NEW.idclient)) THEN
		SIGNAL SQLSTATE VALUE '45000' SET MESSAGE_TEXT = 'Данный пользователь уже зарегестрирован';
	END IF;
END IF;
IF NEW.idworker IS NOT NULL THEN
IF (EXISTS(SELECT 1 FROM logins WHERE idworker = NEW.idworker)) THEN
		SIGNAL SQLSTATE VALUE '45000' SET MESSAGE_TEXT = 'Данный сотрудник уже зарегестрирован';
	END IF;
END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `logins_AFTER_INSERT` AFTER INSERT ON `logins` FOR EACH ROW BEGIN
IF NEW.idclient IS NOT NULL THEN
	insert into journal(idclient, event) values (NEW.idclient, 'Пользователь был зарегистрирован');
END IF;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Temporary view structure for view `view_cards`
--

DROP TABLE IF EXISTS `view_cards`;
/*!50001 DROP VIEW IF EXISTS `view_cards`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_cards` AS SELECT 
 1 AS `iddeposits`,
 1 AS `idclient`,
 1 AS `type`,
 1 AS `pincode`,
 1 AS `cvv2_code`,
 1 AS `duration`,
 1 AS `balance`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_clients`
--

DROP TABLE IF EXISTS `view_clients`;
/*!50001 DROP VIEW IF EXISTS `view_clients`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_clients` AS SELECT 
 1 AS `idclients`,
 1 AS `FIO`,
 1 AS `passportid`,
 1 AS `Address`,
 1 AS `number`,
 1 AS `email`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_deps`
--

DROP TABLE IF EXISTS `view_deps`;
/*!50001 DROP VIEW IF EXISTS `view_deps`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_deps` AS SELECT 
 1 AS `iddeposits`,
 1 AS `idclient`,
 1 AS `type`,
 1 AS `balance`,
 1 AS `duration`,
 1 AS `refill`,
 1 AS `cut`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_journal`
--

DROP TABLE IF EXISTS `view_journal`;
/*!50001 DROP VIEW IF EXISTS `view_journal`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_journal` AS SELECT 
 1 AS `idjournal`,
 1 AS `idclient`,
 1 AS `event`,
 1 AS `date`,
 1 AS `idevent`,
 1 AS `balance`,
 1 AS `operation`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_logins`
--

DROP TABLE IF EXISTS `view_logins`;
/*!50001 DROP VIEW IF EXISTS `view_logins`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_logins` AS SELECT 
 1 AS `login`,
 1 AS `password`,
 1 AS `idclient`,
 1 AS `idworker`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_logins_c`
--

DROP TABLE IF EXISTS `view_logins_c`;
/*!50001 DROP VIEW IF EXISTS `view_logins_c`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_logins_c` AS SELECT 
 1 AS `login`,
 1 AS `password`,
 1 AS `idclient`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_logins_w`
--

DROP TABLE IF EXISTS `view_logins_w`;
/*!50001 DROP VIEW IF EXISTS `view_logins_w`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_logins_w` AS SELECT 
 1 AS `login`,
 1 AS `password`,
 1 AS `idworker`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `view_workers`
--

DROP TABLE IF EXISTS `view_workers`;
/*!50001 DROP VIEW IF EXISTS `view_workers`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `view_workers` AS SELECT 
 1 AS `idworkers`,
 1 AS `FIO`,
 1 AS `access`,
 1 AS `passportid`*/;
SET character_set_client = @saved_cs_client;

--
-- Table structure for table `workers`
--

DROP TABLE IF EXISTS `workers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `workers` (
  `idworkers` int(11) NOT NULL AUTO_INCREMENT,
  `FIO` varchar(60) NOT NULL,
  `access` tinyint(1) DEFAULT '0',
  `passportid` int(11) NOT NULL,
  PRIMARY KEY (`idworkers`),
  UNIQUE KEY `idworkers_UNIQUE` (`idworkers`),
  UNIQUE KEY `passportid_UNIQUE` (`passportid`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `workers`
--

LOCK TABLES `workers` WRITE;
/*!40000 ALTER TABLE `workers` DISABLE KEYS */;
INSERT INTO `workers` VALUES (1,'admin',1,80100015),(5,'Сотрудник 2',0,465652623),(6,'Яворов Евгений',1,906506387);
/*!40000 ALTER TABLE `workers` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,STRICT_ALL_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ALLOW_INVALID_DATES,ERROR_FOR_DIVISION_BY_ZERO,TRADITIONAL,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`root`@`localhost`*/ /*!50003 TRIGGER `workers_AFTER_DELETE` AFTER DELETE ON `workers` FOR EACH ROW BEGIN
DELETE FROM logins WHERE logins.idworker=old.idworkers;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Dumping routines for database 'bankdata'
--
/*!50003 DROP FUNCTION IF EXISTS `auth` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `auth`(f_login varchar(255), f_password varchar(255)) RETURNS int(11)
    DETERMINISTIC
BEGIN 
declare id INT default 0;

SELECT idclient from logins WHERE login=f_login AND password=f_password into id;
 IF id>0 THEN
 insert into journal(idclient, event) values (id, 'Пользователь вошёл в систему');
 END IF;
return id;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `card_duration` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `card_duration`(years INT) RETURNS date
    DETERMINISTIC
BEGIN 
declare old_dur date;
declare new_dur date;

select curdate() into old_dur;

select DATE_ADD(old_dur, INTERVAL years year) into new_dur;

 return new_dur;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `money_card_add` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `money_card_add`(id INT, money decimal(15,2)) RETURNS varchar(12) CHARSET utf8mb4
    DETERMINISTIC
BEGIN 
declare answer VARCHAR(12) default 'fail';
declare client INT default 0;
declare old_balance decimal(15,2) default 0;
declare new_balance decimal(15,2) default 0;

select idclient from cards where iddeposits=id into client;
select balance from cards where iddeposits=id into old_balance;
select (old_balance+money) into new_balance;
UPDATE cards SET balance=new_balance where iddeposits=id;
insert into journal(idclient, event, idevent, balance, operation) values (client, 'Внесение наличных',id,new_balance,money);
return 'OK';

return answer;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `money_card_sub` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `money_card_sub`(id INT, money decimal(15,2)) RETURNS varchar(12) CHARSET utf8mb4
    DETERMINISTIC
BEGIN 
declare answer VARCHAR(12) default 'fail';
declare client INT default 0;
declare old_balance decimal(15,2) default 0;
declare new_balance decimal(15,2) default 0;

select idclient from cards where iddeposits=id into client;
select balance from cards where iddeposits=id into old_balance;
select (old_balance-money) into new_balance;
IF(old_balance<money) THEN return 'fail'; END IF;
UPDATE cards SET balance=new_balance where iddeposits=id;
insert into journal(idclient, event, idevent, balance, operation) values (client, 'Снятие наличных',id,new_balance,money);
return 'OK';

return answer;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `money_dep_add` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `money_dep_add`(id INT, money decimal(15,2)) RETURNS varchar(12) CHARSET utf8mb4
    DETERMINISTIC
BEGIN 
declare answer VARCHAR(12) default 'fail';
declare client INT default 0;
declare refill INT default 0;
declare old_balance decimal(15,2) default 0;
declare new_balance decimal(15,2) default 0;

select idclient from deposits where iddeposits=id into client;

select balance from deposits where iddeposits=id into old_balance;
select (old_balance+money) into new_balance;

UPDATE deposits SET balance=new_balance where iddeposits=id;
insert into journal(idclient, event, idevent, balance, operation) values (client, 'Внесение наличных',id,new_balance,money);
return 'OK';
 
return answer;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `money_dep_sub` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `money_dep_sub`(id INT, money decimal(15,2)) RETURNS varchar(12) CHARSET utf8mb4
    DETERMINISTIC
BEGIN 
declare answer VARCHAR(12) default 'fail';
declare client INT default 0;
declare old_balance decimal(15,2) default 0;
declare new_balance decimal(15,2) default 0;

select idclient from deposits where iddeposits=id into client;

select balance from deposits where iddeposits=id into old_balance;
select (old_balance-money) into new_balance;
	
IF(old_balance<money) THEN return 'fail'; END IF;
UPDATE deposits SET balance=new_balance where iddeposits=id;	
insert into journal(idclient, event, idevent, balance, operation) values (client, 'Снятие наличных',id,new_balance,money);
return 'OK';

return answer;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `transact` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `transact`(id1 INT, id2 INT, money decimal(15,2)) RETURNS varchar(12) CHARSET utf8mb4
    DETERMINISTIC
BEGIN 
declare client1 INT default 0;
declare client2 INT default 0;
declare old_balance decimal(15,2) default 0;
declare new_balance decimal(15,2) default 0;

select idclient from cards where iddeposits=id1 into client1;
select idclient from cards where iddeposits=id2 into client2;

select balance from cards where iddeposits=id1 into old_balance;	
IF(old_balance<money) THEN return 'fail'; END IF;
select (old_balance-money) into new_balance;

UPDATE cards SET balance=new_balance where iddeposits=id1;	
insert into journal(idclient, event, idevent, balance, operation) values (client1, 'Перевод наличных (списание)',id1,new_balance,money);

select balance from cards where iddeposits=id2 into old_balance;
select (old_balance+money) into new_balance;
UPDATE cards SET balance=new_balance where iddeposits=id2;	
insert into journal(idclient, event, idevent, balance, operation) values (client2, 'Перевод наличных (пополнение)',id2,new_balance,money);

return 'OK';

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `tran_phone` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8 */ ;
/*!50003 SET character_set_results = utf8 */ ;
/*!50003 SET collation_connection  = utf8_general_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `tran_phone`(id1 INT, number2 INT, money decimal(15,2)) RETURNS varchar(12) CHARSET utf8mb4
    DETERMINISTIC
BEGIN 
declare client1 INT default 0;
declare client2 INT default 0;
declare check2 INT default 0; -- проверка наличия карт у получателя
declare id2 INT default id1; -- в случае, если у получателя нет карт, деньги вернутся
declare old_balance decimal(15,2) default 0;
declare new_balance decimal(15,2) default 0;

select idclient from cards where iddeposits=id1 into client1;
select idclients from clients where number=number2 into client2;
select iddeposits from cards where idclient=client2 order by balance limit 1 into check2;
IF(check2>0) THEN select check2 into id2; END IF;

select balance from cards where iddeposits=id1 into old_balance;	
IF(old_balance<money) THEN return 'fail'; END IF;
select (old_balance-money) into new_balance;

UPDATE cards SET balance=new_balance where iddeposits=id1;	
insert into journal(idclient, event, idevent, balance, operation) values (client1, 'Перевод наличных (списание)',id1,new_balance,money);

select balance from cards where iddeposits=id2 into old_balance;
select (old_balance+money) into new_balance;
UPDATE cards SET balance=new_balance where iddeposits=id2;	
insert into journal(idclient, event, idevent, balance, operation) values (client2, 'Перевод наличных (пополнение)',id2,new_balance,money);

return 'OK';

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `view_cards`
--

/*!50001 DROP VIEW IF EXISTS `view_cards`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_cards` AS select `cards`.`iddeposits` AS `iddeposits`,`cards`.`idclient` AS `idclient`,`cards`.`type` AS `type`,`cards`.`pincode` AS `pincode`,`cards`.`cvv2_code` AS `cvv2_code`,`cards`.`duration` AS `duration`,`cards`.`balance` AS `balance` from `cards` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_clients`
--

/*!50001 DROP VIEW IF EXISTS `view_clients`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_clients` AS select `clients`.`idclients` AS `idclients`,`clients`.`FIO` AS `FIO`,`clients`.`passportid` AS `passportid`,`clients`.`Address` AS `Address`,`clients`.`number` AS `number`,`clients`.`email` AS `email` from `clients` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_deps`
--

/*!50001 DROP VIEW IF EXISTS `view_deps`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_deps` AS select `deposits`.`iddeposits` AS `iddeposits`,`deposits`.`idclient` AS `idclient`,`deposits`.`type` AS `type`,`deposits`.`balance` AS `balance`,`deposits`.`duration` AS `duration`,`deposits`.`refill` AS `refill`,`deposits`.`cut` AS `cut` from `deposits` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_journal`
--

/*!50001 DROP VIEW IF EXISTS `view_journal`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_journal` AS select `journal`.`idjournal` AS `idjournal`,`journal`.`idclient` AS `idclient`,`journal`.`event` AS `event`,`journal`.`date` AS `date`,`journal`.`idevent` AS `idevent`,`journal`.`balance` AS `balance`,`journal`.`operation` AS `operation` from `journal` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_logins`
--

/*!50001 DROP VIEW IF EXISTS `view_logins`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_logins` AS select `logins`.`login` AS `login`,`logins`.`password` AS `password`,`logins`.`idclient` AS `idclient`,`logins`.`idworker` AS `idworker` from `logins` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_logins_c`
--

/*!50001 DROP VIEW IF EXISTS `view_logins_c`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_logins_c` AS select `logins`.`login` AS `login`,`logins`.`password` AS `password`,`logins`.`idclient` AS `idclient` from `logins` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_logins_w`
--

/*!50001 DROP VIEW IF EXISTS `view_logins_w`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_logins_w` AS select `logins`.`login` AS `login`,`logins`.`password` AS `password`,`logins`.`idworker` AS `idworker` from `logins` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `view_workers`
--

/*!50001 DROP VIEW IF EXISTS `view_workers`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `view_workers` AS select `workers`.`idworkers` AS `idworkers`,`workers`.`FIO` AS `FIO`,`workers`.`access` AS `access`,`workers`.`passportid` AS `passportid` from `workers` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-08-12 11:42:02
