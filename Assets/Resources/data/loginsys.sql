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
  PRIMARY KEY (`id`),
  KEY `INDEX_PLAYER_ID` (`playerId`)
) ENGINE=InnoDB AUTO_INCREMENT=1000003 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `hero` */

insert  into `hero`(`id`,`heroId`,`playerId`,`heroLevel`,`heroJie`,`skillLevel`,`starLevel`) values (1000000,8,1000000,17,1,1,0),(1000001,9,1000000,12,1,1,0),(1000002,11,1000000,2,1,1,0);

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
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1000158 DEFAULT CHARSET=latin1;

/*Data for the table `item` */

insert  into `item`(`id`,`playerId`,`itemId`,`count`,`itemType`,`owerId`,`pos`,`level`,`starLevel`) values (1000000,1000000,101,56805,1,0,0,1,0),(1000001,1000000,1000,5990,7,0,0,1,0),(1000112,1000000,1001,1,2,0,0,1,0),(1000113,1000000,1002,1,2,1000157,1,65535,0),(1000114,1000000,1003,1,2,1000002,1,1,0),(1000116,1000000,1004,1,2,1000003,1,1,0),(1000120,1000000,1001,1,2,1000001,1,1,0),(1000123,1000000,1001,1,2,1000009,1,1,0),(1000126,1000000,1001,1,2,0,0,1,0),(1000129,1000000,1001,1,2,0,0,1,0),(1000130,1000000,1001,1,2,0,0,1,0),(1000131,1000000,1001,1,2,0,0,1,0),(1000134,1000000,1001,1,2,1000000,1,1,0),(1000137,1000000,1001,1,2,0,0,1,0),(1000140,1000000,1001,1,2,0,0,1,0),(1000141,1000000,1001,1,2,0,0,1,0),(1000142,1000000,1001,1,2,0,0,1,0),(1000143,1000000,1001,1,2,0,0,1,0),(1000144,1000000,1001,1,2,0,0,1,0),(1000145,1000000,1001,1,2,0,0,1,0),(1000146,1000000,1001,1,2,0,0,1,0),(1000147,1000000,1001,1,2,0,0,1,0),(1000148,1000000,1001,1,2,0,0,1,0),(1000149,1000000,1001,1,2,0,0,1,0),(1000150,1000000,1001,1,2,0,0,1,0),(1000151,1000000,1001,1,2,0,0,1,0),(1000152,1000000,1001,1,2,0,0,1,0),(1000153,1000000,1001,1,2,0,0,1,0),(1000154,1000000,1001,1,2,0,0,1,0),(1000155,1000000,1001,1,2,0,0,1,0),(1000156,1000000,8089,1,5,0,0,1,0),(1000157,1000000,8089,1,5,8,0,1,0);

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
  `skillPoint` int(10) NOT NULL DEFAULT '0',
  `chapter` varchar(1000) COLLATE utf8_unicode_ci DEFAULT '{}',
  `gold` bigint(20) DEFAULT '100',
  `zuanshi` bigint(20) DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `INDEX_GAME_NAME` (`name`),
  KEY `INDEX_PALYER_USER_ID` (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=1000001 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `player` */

insert  into `player`(`id`,`userId`,`kindId`,`name`,`country`,`rank`,`level`,`experience`,`attackValue`,`defenceValue`,`hitRate`,`dodgeRate`,`walkSpeed`,`attackSpeed`,`hp`,`mp`,`maxHp`,`maxMp`,`areaId`,`x`,`y`,`kindName`,`skillPoint`,`chapter`,`gold`,`zuanshi`) values (1000000,1000000,'210','xiaojunzai',NULL,NULL,NULL,NULL,NULL,NULL,90,13,NULL,NULL,NULL,NULL,NULL,NULL,1,NULL,NULL,'Angle',1,'{\"1\":1,\"2\":1,\"3\":1,\"6\":1,\"11\":1,\"20\":1}',100,0);

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
) ENGINE=InnoDB AUTO_INCREMENT=1000000 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

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
) ENGINE=InnoDB AUTO_INCREMENT=1000001 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

/*Data for the table `user` */

insert  into `user`(`id`,`name`,`password`,`loginCount`,`from`,`lastLoginTime`) values (1000000,'lmj23','123',1,'facebook',1497941288272);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
