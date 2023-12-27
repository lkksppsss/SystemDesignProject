CREATE SCHEMA `spu` ;

CREATE TABLE `spu`.`hotels` (
  `id` int NOT NULL,
  `name` varchar(50) NOT NULL,
  `city` varchar(50) NOT NULL,
  `area` varchar(50) NOT NULL,
  `address` varchar(100) NOT NULL,
  `description` varchar(200) DEFAULT NULL,
  `create_time` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB;

CREATE TABLE `spu`.`hotel_pictures` (
  `id` int NOT NULL AUTO_INCREMENT,
  `hotel_id` int NOT NULL COMMENT 'FK',
  `type` int NOT NULL,
  `url` varchar(200) NOT NULL,
  `create_time` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB ;