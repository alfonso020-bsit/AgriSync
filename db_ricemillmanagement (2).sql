-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Nov 29, 2024 at 05:50 AM
-- Server version: 8.0.30
-- PHP Version: 8.3.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_ricemillmanagement`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_darakpinlid`
--

CREATE TABLE `tbl_darakpinlid` (
  `ID` int NOT NULL,
  `PRODUCTNAME` varchar(255) NOT NULL,
  `QUANTITYPERSACK` int NOT NULL,
  `PRICEPERSACK` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_darakpinlid`
--

INSERT INTO `tbl_darakpinlid` (`ID`, `PRODUCTNAME`, `QUANTITYPERSACK`, `PRICEPERSACK`) VALUES
(1, 'Darak', 59, 500),
(2, 'Pinlid', 80, 500),
(3, 'Mais', 500, 700),
(4, 'Darak', 89, 200),
(5, 'Darak', 89, 1000),
(6, 'Mais', 12, 1000),
(8, 'Pinlid', 120, 120),
(9, 'Pinlid', 100, 50),
(10, 'Pinlid', 100, 100),
(11, 'Darak', 89, 100),
(12, 'Mais', 100, 100);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_inventorydarak`
--

CREATE TABLE `tbl_inventorydarak` (
  `ID` int NOT NULL,
  `CUSTOMERNAME` varchar(255) DEFAULT NULL,
  `QUANTITY` int DEFAULT NULL,
  `PRICE` int DEFAULT NULL,
  `TOTALPRICE` int DEFAULT NULL,
  `DATE` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_inventorydarak`
--

INSERT INTO `tbl_inventorydarak` (`ID`, `CUSTOMERNAME`, `QUANTITY`, `PRICE`, `TOTALPRICE`, `DATE`) VALUES
(1, 'Bautista Alfonso', 70, 20, 1400, '2024-11-01 23:34:57'),
(2, 'Sofhia Castillo', 25, 20, 500, '2024-11-02 09:49:29'),
(3, 'HAHAHAHAHA', 50, 20, 1000, '2024-11-02 12:14:36'),
(4, 'HUHUHUHUH', 60, 25, 1500, '2024-11-02 12:19:24'),
(5, 'DHAHAHAHAH', 43, 20, 860, '2024-11-02 13:28:16'),
(6, 'John Lloyd', 100, 100, 10000, '2024-11-24 13:43:39');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_inventorymais`
--

CREATE TABLE `tbl_inventorymais` (
  `ID` int NOT NULL,
  `CUSTOMERNAME` varchar(255) DEFAULT NULL,
  `QUANTITY` int DEFAULT NULL,
  `PRICE` int DEFAULT NULL,
  `TOTALPRICE` varchar(255) DEFAULT NULL,
  `DATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_inventorymais`
--

INSERT INTO `tbl_inventorymais` (`ID`, `CUSTOMERNAME`, `QUANTITY`, `PRICE`, `TOTALPRICE`, `DATE`) VALUES
(1, 'Salde', 60, 50, '3000', '2024-11-02 13:07:21'),
(2, 'Klien', 50, 50, '2500', '2024-11-02 15:44:54'),
(3, 'John Lloyd', 100, 100, '10000', '2024-11-24 13:44:35');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_inventorypinlid`
--

CREATE TABLE `tbl_inventorypinlid` (
  `ID` int NOT NULL,
  `CUSTOMERNAME` varchar(255) DEFAULT NULL,
  `QUANTITY` int DEFAULT NULL,
  `PRICE` int DEFAULT NULL,
  `TOTALPRICE` int DEFAULT NULL,
  `DATE` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_inventorypinlid`
--

INSERT INTO `tbl_inventorypinlid` (`ID`, `CUSTOMERNAME`, `QUANTITY`, `PRICE`, `TOTALPRICE`, `DATE`) VALUES
(1, 'Hello Pare', 50, 20, 1000, '2024-11-02 12:22:30'),
(6, 'Jelay', 30, 20, 600, '2024-11-02 12:22:53'),
(7, 'John Lloyd', 100, 100, 10000, '2024-11-24 13:44:16');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_inventoryrice`
--

CREATE TABLE `tbl_inventoryrice` (
  `ID` int NOT NULL,
  `CUSTOMERNAME` varchar(255) DEFAULT NULL,
  `VARIETY` varchar(255) DEFAULT NULL,
  `QUANTITY` int DEFAULT NULL,
  `PRICE` int DEFAULT NULL,
  `TOTALPRICE` int DEFAULT NULL,
  `DATE` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_inventoryrice`
--

INSERT INTO `tbl_inventoryrice` (`ID`, `CUSTOMERNAME`, `VARIETY`, `QUANTITY`, `PRICE`, `TOTALPRICE`, `DATE`) VALUES
(1, 'Marieta Alfonso', 'RC 18', 50, 30, 1500, '2024-11-02 11:56:01'),
(2, 'Jayvee Alfonso', 'RC 218', 20, 30, 600, '2024-11-02 11:49:30'),
(3, 'Klien Grutas', 'RC 300 ', 100, 30, 3000, '2024-11-02 11:44:44'),
(4, 'John Lloyd', 'RC 18', 100, 100, 10000, '2024-11-24 13:43:17');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_products`
--

CREATE TABLE `tbl_products` (
  `ID` int NOT NULL,
  `CUSTOMERNAME` varchar(255) NOT NULL,
  `PRODUCTNAME` varchar(255) NOT NULL,
  `VARIETY` varchar(255) DEFAULT NULL,
  `TRANSACTIONTYPE` varchar(255) NOT NULL,
  `QUANTITY` int NOT NULL,
  `PRICE` int NOT NULL,
  `TOTALPRICE` int NOT NULL,
  `DATE` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_products`
--

INSERT INTO `tbl_products` (`ID`, `CUSTOMERNAME`, `PRODUCTNAME`, `VARIETY`, `TRANSACTIONTYPE`, `QUANTITY`, `PRICE`, `TOTALPRICE`, `DATE`) VALUES
(1, 'Syren Calibo', 'Rice', 'RC 18', 'SELL', 5, 1250, 6250, '2024-11-01 15:32:08'),
(2, 'Syren Calibo', 'Rice', 'RC 18', 'SELL', 5, 1250, 6250, '2024-11-05 09:45:59'),
(3, 'Cesar Alfonso', 'Rice', NULL, 'SELL', 10, 1250, 12500, '2024-11-01 13:39:47'),
(5, 'Marieta Alfonso', 'Rice', 'RC 18', 'SELL', 5, 1250, 6250, '2024-11-01 14:24:58'),
(6, 'Jelai', 'Rice', 'RC 18', 'SELL', 10, 1250, 12500, '2024-11-01 14:28:28'),
(7, 'Binay', 'Rice', 'RC 216', 'SELL', 5, 1400, 7000, '2024-11-01 14:37:46'),
(8, 'hahah', 'Rice', 'RC 216', 'SELL', 10, 1400, 14000, '2024-11-01 14:46:44'),
(9, 'hahahah', 'Darak', '', 'SELL', 10, 300, 3000, '2024-11-02 13:37:35'),
(10, 'GAGEGA', 'Darak', '', 'SELL', 10, 300, 3000, '2024-11-01 15:32:22'),
(12, 'gagagaga', 'Pinlid', '', 'SELL', 20, 500, 10000, '2024-11-01 14:55:54'),
(14, 'Salde', 'Rice', 'RC 18', 'SERVICE', 1500, 3, 4500, '2024-11-02 13:37:19'),
(15, 'Sofhia', 'Mais', '', 'SERVICE', 6000, 10, 60000, '2024-11-02 13:38:42'),
(16, 'Klien Grutas', 'Mais', '', 'SERVICE', 700, 3, 2100, '2024-11-02 15:46:30'),
(18, 'John Lloyd', 'Rice', 'RC 18', 'SELL', 100, 100, 10000, '2024-11-24 13:47:19'),
(19, 'John Lloyd', 'Pinlid', '', 'SERVICE', 200, 200, 40000, '2024-11-24 13:48:07'),
(20, 'John Lloyd', 'Mais', '', 'SERVICE', 200, 200, 40000, '2024-11-24 13:48:29'),
(21, 'John Lloyd', 'Darak', '', 'SELL', 11, 50, 550, '2024-11-24 13:49:25');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_rice`
--

CREATE TABLE `tbl_rice` (
  `ID` int NOT NULL,
  `VARIETYNAME` varchar(255) NOT NULL,
  `QUANTITYPERSACK` int NOT NULL,
  `PRICEPERSACK` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_rice`
--

INSERT INTO `tbl_rice` (`ID`, `VARIETYNAME`, `QUANTITYPERSACK`, `PRICEPERSACK`) VALUES
(2, 'RC 218', 10, 1350),
(3, 'RC 216', 40, 1400),
(4, 'RC 436', 100, 1450),
(5, 'RC 18', 10, 1000),
(11, 'RC 300 ', 10, 100),
(12, 'RC 160', 10, 1000),
(13, 'RC 222', 9, 100);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

CREATE TABLE `tbl_users` (
  `ID` int NOT NULL,
  `Username` varchar(50) NOT NULL,
  `FirstName` varchar(50) NOT NULL,
  `LastName` varchar(50) NOT NULL,
  `Password` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Dumping data for table `tbl_users`
--

INSERT INTO `tbl_users` (`ID`, `Username`, `FirstName`, `LastName`, `Password`) VALUES
(1, 'jelai', 'Angelica', 'Alfonso', '111'),
(2, 'John1', 'John Lloyd', 'Jardines', 'Newuser');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_darakpinlid`
--
ALTER TABLE `tbl_darakpinlid`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_inventorydarak`
--
ALTER TABLE `tbl_inventorydarak`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_inventorymais`
--
ALTER TABLE `tbl_inventorymais`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_inventorypinlid`
--
ALTER TABLE `tbl_inventorypinlid`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_inventoryrice`
--
ALTER TABLE `tbl_inventoryrice`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_products`
--
ALTER TABLE `tbl_products`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_rice`
--
ALTER TABLE `tbl_rice`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_darakpinlid`
--
ALTER TABLE `tbl_darakpinlid`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT for table `tbl_inventorydarak`
--
ALTER TABLE `tbl_inventorydarak`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tbl_inventorymais`
--
ALTER TABLE `tbl_inventorymais`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `tbl_inventorypinlid`
--
ALTER TABLE `tbl_inventorypinlid`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT for table `tbl_inventoryrice`
--
ALTER TABLE `tbl_inventoryrice`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `tbl_products`
--
ALTER TABLE `tbl_products`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=22;

--
-- AUTO_INCREMENT for table `tbl_rice`
--
ALTER TABLE `tbl_rice`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `ID` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
