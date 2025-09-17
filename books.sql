-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 04, 2025 at 06:25 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `books`
--
DROP DATABASE IF EXISTS `books`;
CREATE DATABASE `books` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `books`;

-- --------------------------------------------------------

--
-- Table structure for table `author`
--

CREATE TABLE `author` (
  `id` int(6) NOT NULL,
  `name` varchar(80) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `author`
--

INSERT INTO `author` (`id`, `name`) VALUES
(11, 'Andy Weir'),
(18, 'Antoine de Saint-Exupéry'),
(25, 'Arthur Conan Doyle'),
(6, 'Bud Spencer'),
(23, 'Dan Brown'),
(14, 'Ernest Hemingway'),
(4, 'Fekete István'),
(13, 'Fjodor Mihajlovics Dosztojevszkij'),
(10, 'Frank Herbert'),
(16, 'Gabriel García Márquez'),
(1, 'Gárdonyi Géza'),
(15, 'George Orwell'),
(24, 'Herman Melville'),
(9, 'J.K. Rowling'),
(8, 'J.R.R. Tolkien'),
(22, 'John Green'),
(2, 'Jókai Mór'),
(12, 'Joseph Heller'),
(26, 'Jostein Gaarder'),
(21, 'Madách Imre'),
(7, 'Margaret Atwood'),
(17, 'Mihail Bulgakov'),
(3, 'Molnár Ferenc'),
(5, 'Móricz Zsigmond'),
(20, 'Ray Bradbury'),
(19, 'Stephen Hawking');

-- --------------------------------------------------------

--
-- Table structure for table `book`
--

CREATE TABLE `book` (
  `id` int(6) NOT NULL,
  `title` varchar(80) NOT NULL,
  `authorId` int(6) NOT NULL,
  `year` int(4) NOT NULL,
  `pages` int(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `book`
--

INSERT INTO `book` (`id`, `title`, `authorId`, `year`, `pages`) VALUES
(1, 'Egri csillagok', 1, 1899, 600),
(2, 'Az arany ember', 2, 1872, 470),
(3, 'A Pál utcai fiúk', 3, 1906, 210),
(4, 'Tüskevár', 4, 1957, 320),
(5, 'A koszívu ember fiai', 2, 1869, 544),
(6, 'Légy jó mindhalálig', 5, 1920, 380),
(7, 'Piedone nyomában', 6, 2010, 420),
(8, 'A szolgálólány meséje', 7, 1985, 450),
(9, 'A Gyuruk Ura', 8, 1954, 1200),
(10, 'Harry Potter és a bölcsek köve', 9, 1997, 345),
(11, 'Dune', 10, 1965, 800),
(12, 'A marsi', 11, 2011, 385),
(13, 'A 22-es csapdája', 12, 1961, 560),
(14, 'Bun és bunhodés', 13, 1866, 672),
(15, 'Az öreg halász és a tenger', 14, 1952, 127),
(16, '1984', 15, 1949, 328),
(17, 'Állatfarm', 15, 1945, 140),
(18, 'Száz év magány', 16, 1967, 417),
(19, 'A mester és Margarita', 17, 1967, 420),
(20, 'A kis herceg', 18, 1943, 96),
(21, 'Az ido rövid története', 19, 1988, 212),
(22, 'Fahrenheit 451', 20, 1953, 256),
(23, 'Az ember tragédiája', 21, 1861, 250),
(24, 'Csillagainkban a hiba', 22, 2012, 313),
(25, 'A Da Vinci-kód', 23, 2003, 689),
(26, 'Inferno', 23, 2013, 624),
(27, 'A Hobbit', 8, 1937, 310),
(28, 'Moby Dick', 24, 1851, 635),
(29, 'Sherlock Holmes kalandjai', 25, 1892, 307),
(30, 'Vita Brevis', 26, 1996, 192),
(31, 'Ida regénye', 1, 1920, 300),
(32, 'Egy magyar nábob', 2, 1853, 464),
(33, 'Liliom', 3, 1909, 170),
(34, 'Téli berek', 4, 1959, 310),
(35, 'Rokonok', 5, 1932, 368),
(36, 'Mindent apámról', 6, 2016, 380),
(37, 'Alias Grace', 7, 1996, 480),
(38, 'A szilmarilok', 8, 1977, 365),
(39, 'Harry Potter és a titkok kamrája', 9, 1998, 366),
(40, 'A Dune messiása', 10, 1969, 256),
(41, 'Project Hail Mary', 11, 2021, 496),
(42, 'Valami történt', 12, 1974, 569),
(43, 'A félkegyelmu', 13, 1869, 640),
(44, 'Akiért a harang szól', 14, 1940, 480),
(45, 'Hódolat Katalóniának', 15, 1938, 232),
(46, 'Szerelem a kolera idején', 16, 1985, 348),
(47, 'Kutyaszív', 17, 1925, 160),
(48, 'Éjszakai repülés', 18, 1931, 120),
(49, 'A nagy terv', 19, 2010, 208),
(50, 'Marsbéli krónikák', 20, 1950, 256),
(51, 'Mózes', 21, 1861, 200),
(52, 'Alaska', 22, 2005, 221),
(53, 'Angyalok és démonok', 23, 2000, 616),
(54, 'A babó', 8, 1937, 310),
(55, 'Billy Budd', 24, 1924, 160),
(56, 'A sátán kutyája', 25, 1902, 256),
(57, 'Sofie világa', 26, 1991, 512);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `author`
--
ALTER TABLE `author`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `name` (`name`);

--
-- Indexes for table `book`
--
ALTER TABLE `book`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `title` (`title`),
  ADD KEY `authorId` (`authorId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `author`
--
ALTER TABLE `author`
  MODIFY `id` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `book`
--
ALTER TABLE `book`
  MODIFY `id` int(6) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=58;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
