/*
SQLyog Ultimate v12.09 (64 bit)
MySQL - 5.6.26 : Database - pomelo
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`pomelo` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `pomelo`;

/*Table structure for table `hero` */

DROP TABLE IF EXISTS `hero`;

CREATE TABLE `hero` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `heroId` bigint(20) unsigned NOT NULL DEFAULT '0',
  `playerId` bigint(20) unsigned NOT NULL DEFAULT '0',
  `heroLevel` smallint(6) unsigned DEFAULT '0',
  `heroJie` smallint(6) unsigned DEFAULT '0',
  `skillLevel` smallint(6) unsigned DEFAULT '0',
  `starLevel` smallint(6) unsigned NOT NULL DEFAULT '0',
  `fightPoint` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `INDEX_PLAYER_ID` (`playerId`)
) ENGINE=InnoDB AUTO_INCREMENT=1000024 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `hero` */

insert  into `hero`(`id`,`heroId`,`playerId`,`heroLevel`,`heroJie`,`skillLevel`,`starLevel`,`fightPoint`) values (1000000,8,1000000,20,1,1,0,0),(1000001,9,1000000,19,1,1,0,0),(1000002,11,1000000,13,1,1,0,16990),(1000003,8,1000001,1,1,1,0,0),(1000004,9,1000001,1,1,1,0,0),(1000005,11,1000001,1,1,1,0,0),(1000006,8,1000002,1,1,1,0,0),(1000007,9,1000002,1,1,1,0,0),(1000008,11,1000002,1,1,1,0,0),(1000009,8,1000003,1,1,1,0,0),(1000010,9,1000003,1,1,1,0,0),(1000011,11,1000003,1,1,1,0,0),(1000012,8,1000004,1,1,1,0,0),(1000013,9,1000004,1,1,1,0,0),(1000014,11,1000004,1,1,1,0,0),(1000015,8,1000005,1,1,1,0,0),(1000016,9,1000005,1,1,1,0,0),(1000017,11,1000005,1,1,1,0,0),(1000018,8,1000006,1,1,1,0,0),(1000019,9,1000006,1,1,1,0,0),(1000020,11,1000006,1,1,1,0,0),(1000021,8,1000007,1,1,1,0,0),(1000022,9,1000007,1,1,1,0,0),(1000023,11,1000007,1,1,1,0,0);

/*Table structure for table `item` */

DROP TABLE IF EXISTS `item`;

CREATE TABLE `item` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `playerId` bigint(20) unsigned NOT NULL,
  `itemId` bigint(20) unsigned NOT NULL,
  `count` smallint(6) unsigned NOT NULL,
  `itemType` smallint(6) unsigned NOT NULL DEFAULT '1',
  `owerId` bigint(20) unsigned NOT NULL DEFAULT '0',
  `pos` smallint(6) unsigned NOT NULL DEFAULT '0',
  `level` smallint(6) unsigned NOT NULL DEFAULT '1',
  `starLevel` smallint(6) unsigned NOT NULL DEFAULT '0',
  `fightPoint` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000475 DEFAULT CHARSET=latin1;

/*Data for the table `item` */

insert  into `item`(`id`,`playerId`,`itemId`,`count`,`itemType`,`owerId`,`pos`,`level`,`starLevel`,`fightPoint`) values (1000000,1000000,101,59145,1,0,0,1,0,0),(1000001,1000000,1000,2150,7,0,0,1,0,0),(1000113,1000000,1025,1,2,1000174,1,1,0,0),(1000116,1000000,1004,1,2,1000166,1,1,0,0),(1000156,1000000,8089,1,5,9,0,7,0,1110),(1000157,1000000,8089,1,5,8,0,12,0,1210),(1000158,1000000,3013,2,9,0,0,1,0,0),(1000159,1000000,8103,1,5,8,0,12,0,1310),(1000160,1000000,8103,1,5,11,0,1,0,0),(1000161,1000000,8103,1,5,9,0,1,0,0),(1000162,1000000,8103,1,5,0,0,9,0,1145),(1000163,1000000,8103,1,5,0,0,1,0,0),(1000164,1000000,8103,1,5,0,0,5,0,925),(1000165,1000000,8004,1,5,8,0,1,0,1115),(1000166,1000000,8004,1,5,9,0,1,0,965),(1000167,1000000,8004,1,5,0,0,4,0,1160),(1000168,1000000,8004,1,5,0,0,1,0,0),(1000169,1000000,8004,1,5,0,0,1,0,0),(1000170,1000000,8004,1,5,0,0,1,0,0),(1000171,1000000,8004,1,5,0,0,1,0,0),(1000172,1000000,8004,1,5,0,0,8,0,1420),(1000173,1000000,8004,1,5,0,0,1,0,0),(1000174,1000000,8015,1,5,11,0,10,0,0),(1000175,1000000,8015,1,5,9,0,1,0,0),(1000176,1000000,8015,1,5,8,0,7,0,1155),(1000177,1000000,8004,1,5,0,0,9,0,1485),(1000319,1000000,1019,1,2,1000174,2,1,0,0),(1000459,1000000,1019,1,2,1000165,1,1,0,0),(1000464,1000000,1007,1,2,1000156,1,1,0,0),(1000465,1000000,3001,1,9,0,0,1,0,0),(1000466,1000000,1001,1,2,0,0,1,0,0),(1000467,1000000,1001,1,2,0,0,1,0,0),(1000468,1000000,1001,1,2,0,0,1,0,0),(1000469,1000000,1001,1,2,0,0,1,0,0),(1000470,1000000,1001,1,2,0,0,1,0,0),(1000471,1000000,1001,1,2,0,0,1,0,0),(1000472,1000000,1001,1,2,0,0,1,0,0),(1000473,1000000,1001,1,2,0,0,1,0,0),(1000474,1000000,1001,1,2,0,0,1,0,0);

/*Table structure for table `name_role` */

DROP TABLE IF EXISTS `name_role`;

CREATE TABLE `name_role` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(32) DEFAULT NULL,
  `state` int(11) DEFAULT NULL,
  `group_id` int(11) DEFAULT NULL,
  `create_time` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=1000000 DEFAULT CHARSET=utf8;

/*Data for the table `name_role` */

/*Table structure for table `player` */

DROP TABLE IF EXISTS `player`;

CREATE TABLE `player` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `userId` bigint(20) unsigned NOT NULL DEFAULT '0',
  `kindId` varchar(10) COLLATE utf8_unicode_ci DEFAULT '0002',
  `name` varchar(50) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `country` smallint(6) unsigned DEFAULT '0',
  `rank` smallint(6) unsigned DEFAULT '1' COMMENT 'dfsfds',
  `level` smallint(6) unsigned DEFAULT '1',
  `experience` smallint(11) unsigned DEFAULT '0',
  `attackValue` smallint(6) unsigned DEFAULT '0',
  `defenceValue` smallint(6) unsigned DEFAULT '0',
  `hitRate` smallint(6) unsigned DEFAULT '0',
  `dodgeRate` smallint(6) unsigned DEFAULT '0',
  `walkSpeed` smallint(6) unsigned DEFAULT '0',
  `attackSpeed` smallint(6) unsigned DEFAULT '0',
  `hp` smallint(6) unsigned DEFAULT '0',
  `mp` smallint(6) unsigned DEFAULT '0',
  `maxHp` smallint(6) unsigned DEFAULT '0',
  `maxMp` smallint(6) unsigned DEFAULT '0',
  `areaId` bigint(20) unsigned DEFAULT '1',
  `x` int(10) unsigned DEFAULT '0',
  `y` int(10) unsigned DEFAULT '0',
  `kindName` varchar(30) COLLATE utf8_unicode_ci DEFAULT 'god soilder',
  `fightPoint` int(20) unsigned NOT NULL DEFAULT '0',
  `chapter` smallint(6) unsigned NOT NULL DEFAULT '0',
  `gold` bigint(20) DEFAULT '100',
  `zuanshi` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `INDEX_GAME_NAME` (`name`),
  KEY `INDEX_PALYER_USER_ID` (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=1000008 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `player` */

insert  into `player`(`id`,`userId`,`kindId`,`name`,`country`,`rank`,`level`,`experience`,`attackValue`,`defenceValue`,`hitRate`,`dodgeRate`,`walkSpeed`,`attackSpeed`,`hp`,`mp`,`maxHp`,`maxMp`,`areaId`,`x`,`y`,`kindName`,`fightPoint`,`chapter`,`gold`,`zuanshi`) values (1000000,1000000,'210','xiaojunzai',NULL,NULL,NULL,NULL,NULL,NULL,90,13,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,'Angle',59735,3,NULL,NULL),(1000001,1000001,'210','asfasf',NULL,NULL,1,0,23,9,90,13,240,1,220,20,220,20,1,384,177,'Angle',36600,0,100,0),(1000002,1000002,'210','qwrqwr',NULL,NULL,1,0,23,9,90,13,240,1,220,20,220,20,1,454,125,'Angle',1,0,100,0),(1000003,1000003,'210','asfdasf',NULL,NULL,1,0,23,9,90,13,240,1,220,20,220,20,1,402,119,'Angle',1,0,100,0),(1000004,1000004,'210','fsefgwe',1,1,1,0,23,9,90,13,240,1,220,20,220,20,1,435,156,'Angle',1,0,100,0),(1000005,1000005,'210','asdfasdf',1,1,1,0,23,9,90,13,240,1,220,20,220,20,1,350,163,'Angle',1,0,100,0),(1000006,1000006,'210','asdfasfas',NULL,NULL,1,0,23,9,90,13,240,1,220,20,220,20,1,401,114,'Angle',36600,0,100,0),(1000007,1000007,'210','asfasfsads',1,1,1,0,23,9,90,13,240,1,220,20,220,20,1,359,150,'Angle',1,0,100,0);

/*Table structure for table `task` */

DROP TABLE IF EXISTS `task`;

CREATE TABLE `task` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `playerId` bigint(20) unsigned NOT NULL DEFAULT '0',
  `kindId` bigint(20) unsigned NOT NULL DEFAULT '0',
  `taskState` smallint(6) unsigned DEFAULT '0',
  `startTime` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `taskData` varchar(1000) COLLATE utf8_unicode_ci DEFAULT '{}',
  PRIMARY KEY (`id`),
  KEY `INDEX_TASK_ID` (`playerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `task` */

/*Table structure for table `user` */

DROP TABLE IF EXISTS `user`;

CREATE TABLE `user` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(50) COLLATE utf8_unicode_ci DEFAULT '',
  `loginCount` smallint(6) unsigned DEFAULT '0',
  `from` varchar(25) COLLATE utf8_unicode_ci DEFAULT NULL,
  `lastLoginTime` bigint(20) unsigned DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `INDEX_ACCOUNT_NAME` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=1000008 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `user` */

insert  into `user`(`id`,`name`,`password`,`loginCount`,`from`,`lastLoginTime`) values (1000000,'lmj23','123',1,'facebook',1497941288272),(1000001,'lmj231','123',1,'facebook',1498551645256),(1000002,'lmj123','123',1,'facebook',1498552144407),(1000003,'lmj234','123',1,'facebook',1498552340515),(1000004,'lmj233','123',1,'facebook',1498552419676),(1000005,'lmj451','123',1,'facebook',1498552925271),(1000006,'lmj2345','123',1,'facebook',1498553013836),(1000007,'lmj4534','123',1,'facebook',1498553066654);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
