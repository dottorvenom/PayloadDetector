-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Creato il: Mar 28, 2023 alle 13:06
-- Versione del server: 10.4.27-MariaDB
-- Versione PHP: 8.2.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `c2`
--

-- --------------------------------------------------------

--
-- Struttura della tabella `log_keylog`
--

CREATE TABLE `log_keylog` (
  `id` int(11) NOT NULL,
  `ip_address` varchar(200) NOT NULL,
  `b64` longtext NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `hostname` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struttura della tabella `log_monitor`
--

CREATE TABLE `log_monitor` (
  `id` int(11) NOT NULL,
  `ip_address_source` varchar(200) NOT NULL,
  `id_log` varchar(5) NOT NULL,
  `b64` longtext NOT NULL,
  `timestamp` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `hash` varchar(200) NOT NULL,
  `file_name` varchar(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struttura della tabella `tbl_ips`
--

CREATE TABLE `tbl_ips` (
  `id` int(11) NOT NULL,
  `ip_address` varchar(200) NOT NULL,
  `hostname` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struttura della tabella `tbl_ips_process`
--

CREATE TABLE `tbl_ips_process` (
  `id` int(11) NOT NULL,
  `id_parent_ip` int(11) NOT NULL,
  `process_name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Struttura della tabella `tbl_ips_process_dll`
--

CREATE TABLE `tbl_ips_process_dll` (
  `id` int(11) NOT NULL,
  `id_parent_proc` int(11) NOT NULL,
  `id_parent_ip` int(11) NOT NULL,
  `dll_path` varchar(200) NOT NULL,
  `hash` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indici per le tabelle scaricate
--

--
-- Indici per le tabelle `log_keylog`
--
ALTER TABLE `log_keylog`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `log_monitor`
--
ALTER TABLE `log_monitor`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `tbl_ips`
--
ALTER TABLE `tbl_ips`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `tbl_ips_process`
--
ALTER TABLE `tbl_ips_process`
  ADD PRIMARY KEY (`id`);

--
-- Indici per le tabelle `tbl_ips_process_dll`
--
ALTER TABLE `tbl_ips_process_dll`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT per le tabelle scaricate
--

--
-- AUTO_INCREMENT per la tabella `log_keylog`
--
ALTER TABLE `log_keylog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `log_monitor`
--
ALTER TABLE `log_monitor`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `tbl_ips`
--
ALTER TABLE `tbl_ips`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `tbl_ips_process`
--
ALTER TABLE `tbl_ips_process`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT per la tabella `tbl_ips_process_dll`
--
ALTER TABLE `tbl_ips_process_dll`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
