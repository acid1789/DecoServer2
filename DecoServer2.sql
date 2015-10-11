-- MySQL dump 10.13  Distrib 5.6.24, for Win64 (x86_64)
--
-- Host: localhost    Database: deco
-- ------------------------------------------------------
-- Server version	5.6.26-log

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
-- Table structure for table `accounts`
--

DROP TABLE IF EXISTS `accounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accounts` (
  `account_id` int(11) NOT NULL AUTO_INCREMENT,
  `user_name` varchar(64) NOT NULL,
  `password` varchar(64) NOT NULL,
  PRIMARY KEY (`account_id`),
  UNIQUE KEY `account_id_UNIQUE` (`account_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `char_skills`
--

DROP TABLE IF EXISTS `char_skills`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `char_skills` (
  `character_id` int(10) unsigned NOT NULL,
  `skill_id` smallint(5) unsigned NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `characters`
--

DROP TABLE IF EXISTS `characters`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters` (
  `character_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(16) NOT NULL,
  `model_info` int(11) DEFAULT '0',
  `job` tinyint(4) DEFAULT '0',
  `level` tinyint(4) DEFAULT '1',
  `shirt` int(11) DEFAULT '0',
  `pants` int(11) DEFAULT '0',
  `right_hand` int(11) DEFAULT '0',
  `left_hand` int(11) DEFAULT '0',
  `hat` int(11) DEFAULT '0',
  `suit` int(11) DEFAULT '0',
  `gloves` int(11) DEFAULT '0',
  `boots` int(11) DEFAULT '0',
  `neck1` int(11) DEFAULT '0',
  `neck2` int(11) DEFAULT '0',
  `account_id` int(11) NOT NULL,
  PRIMARY KEY (`character_id`),
  UNIQUE KEY `character_id_UNIQUE` (`character_id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `characters_hv`
--

DROP TABLE IF EXISTS `characters_hv`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters_hv` (
  `character_id` int(10) unsigned NOT NULL,
  `map_id` smallint(5) unsigned NOT NULL,
  `cell_index` int(10) unsigned NOT NULL,
  `hp` int(10) unsigned NOT NULL,
  `sp` int(10) unsigned NOT NULL,
  `mp` int(10) unsigned NOT NULL,
  `gold` int(10) unsigned NOT NULL,
  `pvp_wins` int(10) unsigned DEFAULT '0',
  `pvp_count` int(10) unsigned DEFAULT '0',
  `exp` bigint(20) unsigned DEFAULT '0',
  PRIMARY KEY (`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `characters_lv`
--

DROP TABLE IF EXISTS `characters_lv`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters_lv` (
  `character_id` int(10) unsigned NOT NULL,
  `fame` int(10) unsigned NOT NULL,
  `nation_rate` int(10) unsigned NOT NULL,
  `move_speed` smallint(5) unsigned NOT NULL,
  `ability_p_min` smallint(5) unsigned NOT NULL,
  `ability_p_max` smallint(5) unsigned NOT NULL,
  `attack_speed` tinyint(3) unsigned NOT NULL,
  `ability_m_min` smallint(5) unsigned NOT NULL,
  `ability_m_max` smallint(5) unsigned NOT NULL,
  `hp` int(10) unsigned NOT NULL,
  `sp` int(10) unsigned NOT NULL,
  `mp` int(10) unsigned NOT NULL,
  `magical_def` smallint(5) unsigned NOT NULL,
  `physical_def` smallint(5) unsigned NOT NULL,
  `power` smallint(5) unsigned NOT NULL,
  `vitality` smallint(5) unsigned NOT NULL,
  `sympathy` smallint(5) unsigned NOT NULL,
  `intelligence` smallint(5) unsigned NOT NULL,
  `stamina` smallint(5) unsigned NOT NULL,
  `dexterity` smallint(5) unsigned NOT NULL,
  `charisma` tinyint(3) unsigned NOT NULL,
  `luck` tinyint(3) unsigned NOT NULL,
  `ability_points` smallint(5) unsigned NOT NULL,
  `left_sp` smallint(5) unsigned NOT NULL,
  `total_sp` smallint(5) unsigned NOT NULL,
  `frontier_id` int(10) DEFAULT '0',
  PRIMARY KEY (`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `frontiers`
--

DROP TABLE IF EXISTS `frontiers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `frontiers` (
  `frontier_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(14) NOT NULL,
  `level` tinyint(3) unsigned NOT NULL DEFAULT '1',
  `membership_fee` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`frontier_id`),
  UNIQUE KEY `frontier_id_UNIQUE` (`frontier_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `item_instances`
--

DROP TABLE IF EXISTS `item_instances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `item_instances` (
  `instance_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `template_id` int(10) unsigned NOT NULL,
  `durability` smallint(5) unsigned NOT NULL,
  `remaining_time` smallint(5) unsigned NOT NULL,
  `character_id` int(10) unsigned NOT NULL,
  `inventory_type` int(10) unsigned NOT NULL,
  PRIMARY KEY (`instance_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-10-11 10:11:57
