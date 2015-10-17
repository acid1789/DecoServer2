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
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accounts`
--

LOCK TABLES `accounts` WRITE;
/*!40000 ALTER TABLE `accounts` DISABLE KEYS */;
INSERT INTO `accounts` VALUES (1,'a','a'),(2,'acid1789','test');
/*!40000 ALTER TABLE `accounts` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `active_quests`
--

DROP TABLE IF EXISTS `active_quests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `active_quests` (
  `character_id` int(10) unsigned NOT NULL,
  `quest_id` int(11) unsigned NOT NULL,
  `step` tinyint(3) unsigned DEFAULT '0',
  PRIMARY KEY (`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `active_quests`
--

LOCK TABLES `active_quests` WRITE;
/*!40000 ALTER TABLE `active_quests` DISABLE KEYS */;
INSERT INTO `active_quests` VALUES (5,1,0);
/*!40000 ALTER TABLE `active_quests` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `char_skills`
--

LOCK TABLES `char_skills` WRITE;
/*!40000 ALTER TABLE `char_skills` DISABLE KEYS */;
/*!40000 ALTER TABLE `char_skills` ENABLE KEYS */;
UNLOCK TABLES;

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
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characters`
--

LOCK TABLES `characters` WRITE;
/*!40000 ALTER TABLE `characters` DISABLE KEYS */;
INSERT INTO `characters` VALUES (4,'Pink',1324917010,1,1,0,0,0,0,0,0,0,0,0,0,1),(5,'MeillenaF',1324261623,0,1,0,0,0,0,0,0,0,0,0,0,1);
/*!40000 ALTER TABLE `characters` ENABLE KEYS */;
UNLOCK TABLES;

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
  `fame` int(10) unsigned DEFAULT '0',
  PRIMARY KEY (`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `characters_hv`
--

LOCK TABLES `characters_hv` WRITE;
/*!40000 ALTER TABLE `characters_hv` DISABLE KEYS */;
INSERT INTO `characters_hv` VALUES (4,7,113885,150,150,150,123,0,0,0,0),(5,5,13447,150,150,150,123,0,0,0,0);
/*!40000 ALTER TABLE `characters_hv` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `characters_lv`
--

DROP TABLE IF EXISTS `characters_lv`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `characters_lv` (
  `character_id` int(10) unsigned NOT NULL,
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
-- Dumping data for table `characters_lv`
--

LOCK TABLES `characters_lv` WRITE;
/*!40000 ALTER TABLE `characters_lv` DISABLE KEYS */;
INSERT INTO `characters_lv` VALUES (4,0,192,10,12,8,10,12,150,150,150,5,2,13,9,13,12,12,13,0,0,0,0,100,0),(5,0,192,10,12,8,10,12,150,150,150,2,5,13,9,13,12,12,13,0,0,0,0,100,0);
/*!40000 ALTER TABLE `characters_lv` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `completed_quests`
--

DROP TABLE IF EXISTS `completed_quests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `completed_quests` (
  `character_id` int(10) unsigned NOT NULL,
  `quest_id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`character_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `completed_quests`
--

LOCK TABLES `completed_quests` WRITE;
/*!40000 ALTER TABLE `completed_quests` DISABLE KEYS */;
/*!40000 ALTER TABLE `completed_quests` ENABLE KEYS */;
UNLOCK TABLES;

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
-- Dumping data for table `frontiers`
--

LOCK TABLES `frontiers` WRITE;
/*!40000 ALTER TABLE `frontiers` DISABLE KEYS */;
/*!40000 ALTER TABLE `frontiers` ENABLE KEYS */;
UNLOCK TABLES;

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

--
-- Dumping data for table `item_instances`
--

LOCK TABLES `item_instances` WRITE;
/*!40000 ALTER TABLE `item_instances` DISABLE KEYS */;
/*!40000 ALTER TABLE `item_instances` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `play_maps`
--

DROP TABLE IF EXISTS `play_maps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `play_maps` (
  `map_id` smallint(6) unsigned NOT NULL,
  PRIMARY KEY (`map_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `play_maps`
--

LOCK TABLES `play_maps` WRITE;
/*!40000 ALTER TABLE `play_maps` DISABLE KEYS */;
INSERT INTO `play_maps` VALUES (5),(7);
/*!40000 ALTER TABLE `play_maps` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_info`
--

DROP TABLE IF EXISTS `quest_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_info` (
  `quest_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `giver_id` int(10) unsigned NOT NULL,
  `giver_map_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`quest_id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_info`
--

LOCK TABLES `quest_info` WRITE;
/*!40000 ALTER TABLE `quest_info` DISABLE KEYS */;
INSERT INTO `quest_info` VALUES (1,1,5);
/*!40000 ALTER TABLE `quest_info` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_lines`
--

DROP TABLE IF EXISTS `quest_lines`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_lines` (
  `quest_line_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `quest_id` int(10) unsigned NOT NULL,
  `step` tinyint(3) unsigned NOT NULL,
  `line` tinyint(3) unsigned NOT NULL,
  `icon` smallint(5) unsigned NOT NULL,
  `static_text` smallint(5) unsigned NOT NULL DEFAULT '0',
  `text` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`quest_line_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_lines`
--

LOCK TABLES `quest_lines` WRITE;
/*!40000 ALTER TABLE `quest_lines` DISABLE KEYS */;
INSERT INTO `quest_lines` VALUES (1,1,0,0,2193,1184,NULL),(2,1,0,1,2193,1185,NULL),(3,1,0,2,2193,1186,NULL),(4,1,0,3,2193,1187,NULL),(5,1,0,4,2193,1188,NULL),(6,1,0,5,2193,1189,NULL),(7,1,0,6,2193,1190,NULL),(8,1,0,7,2193,1191,NULL),(9,1,0,8,2193,1192,NULL);
/*!40000 ALTER TABLE `quest_lines` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_requirements`
--

DROP TABLE IF EXISTS `quest_requirements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_requirements` (
  `quest_id` int(10) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL,
  `param` int(10) unsigned DEFAULT '0',
  PRIMARY KEY (`quest_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_requirements`
--

LOCK TABLES `quest_requirements` WRITE;
/*!40000 ALTER TABLE `quest_requirements` DISABLE KEYS */;
/*!40000 ALTER TABLE `quest_requirements` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_rewards`
--

DROP TABLE IF EXISTS `quest_rewards`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_rewards` (
  `quest_id` int(10) unsigned NOT NULL,
  `step` tinyint(3) unsigned NOT NULL,
  `gold` int(10) unsigned DEFAULT '0',
  `exp` int(10) unsigned DEFAULT '0',
  `item` smallint(5) unsigned DEFAULT '0',
  `preward` tinyint(3) unsigned DEFAULT '0',
  `fame` int(10) unsigned DEFAULT '0',
  PRIMARY KEY (`quest_id`,`step`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_rewards`
--

LOCK TABLES `quest_rewards` WRITE;
/*!40000 ALTER TABLE `quest_rewards` DISABLE KEYS */;
INSERT INTO `quest_rewards` VALUES (1,0,0,0,4,0,0);
/*!40000 ALTER TABLE `quest_rewards` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `quest_steps`
--

DROP TABLE IF EXISTS `quest_steps`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest_steps` (
  `quest_id` int(10) unsigned NOT NULL,
  `step` tinyint(3) unsigned NOT NULL,
  `type` tinyint(3) unsigned NOT NULL,
  `count` int(10) unsigned DEFAULT '0',
  `target_id` int(10) unsigned DEFAULT '0',
  `owner_id` int(10) unsigned DEFAULT '0',
  PRIMARY KEY (`quest_id`,`step`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `quest_steps`
--

LOCK TABLES `quest_steps` WRITE;
/*!40000 ALTER TABLE `quest_steps` DISABLE KEYS */;
INSERT INTO `quest_steps` VALUES (1,0,3,0,1,1);
/*!40000 ALTER TABLE `quest_steps` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `static_npcs`
--

DROP TABLE IF EXISTS `static_npcs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `static_npcs` (
  `static_npc_id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `game_id` smallint(5) unsigned NOT NULL,
  `location_x` int(10) unsigned NOT NULL,
  `location_y` int(10) unsigned NOT NULL,
  `hp` int(10) unsigned NOT NULL DEFAULT '10000',
  `direction` int(10) unsigned NOT NULL DEFAULT '0',
  `map_id` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`static_npc_id`),
  UNIQUE KEY `static_npc_id_UNIQUE` (`static_npc_id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `static_npcs`
--

LOCK TABLES `static_npcs` WRITE;
/*!40000 ALTER TABLE `static_npcs` DISABLE KEYS */;
INSERT INTO `static_npcs` VALUES (1,5027,128,60,10000,270,5),(2,5029,150,145,10000,40,5);
/*!40000 ALTER TABLE `static_npcs` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2015-10-17  9:41:06
