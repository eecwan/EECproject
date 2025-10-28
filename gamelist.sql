-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- 主機： 127.0.0.1
-- 產生時間： 2025-10-27 06:20:30
-- 伺服器版本： 10.4.32-MariaDB
-- PHP 版本： 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 資料庫： `mydb`
--

-- --------------------------------------------------------

--
-- 資料表結構 `gamelist`
--

CREATE TABLE `gamelist` (
  `GameID` int(11) NOT NULL,
  `GameNameTW` varchar(200) NOT NULL,
  `GameNameEN` varchar(200) NOT NULL,
  `GameImageUrl` varchar(500) DEFAULT NULL,
  `GameCode` varchar(50) NOT NULL,
  `ReleaseDate` datetime DEFAULT NULL,
  `GameCategory` varchar(100) DEFAULT NULL,
  `IsPromoted` tinyint(1) DEFAULT 0,
  `IsActive` tinyint(1) DEFAULT 1,
  `GameLink` varchar(500) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- 傾印資料表的資料 `gamelist`
--

INSERT INTO `gamelist` (`GameID`, `GameNameTW`, `GameNameEN`, `GameImageUrl`, `GameCode`, `ReleaseDate`, `GameCategory`, `IsPromoted`, `IsActive`, `GameLink`) VALUES
(1, 'MuertosMultiplierMegaways', 'MuertosMultiplierMegaways', 'https://content001.bet365.mx/Games/SGP/GamePodImages/MuertosMultiplierMegaways_1000x400_With-Character.jpg', 'G10161730', '2025-10-15 00:00:00', '獨創', 1, 1, ''),
(3, 'BingOReely', 'BingOReely', 'https://content001.bet365.mx/Games/SGP/GamePodImages/BingOReely_1000x400_With-Character.jpg', 'G10171730', '2025-10-02 00:00:00', '獨創', 1, 1, ''),
(4, 'MercenaryMultiMaxbet365', 'MercenaryMultiMaxbet365', 'https://content001.bet365.mx/Games/SGP/GamePodImages/MercenaryMultiMaxbet365_1000x400_With-Character.jpg', 'G10171735', '2025-10-10 00:00:00', '獨創', 1, 1, ''),
(5, 'LynnOReely', 'LynnOReely', 'https://content001.bet365.mx/Games/SGP/GamePodImages/LynnOReely_1000x400_With-Character.jpg', 'G10171744', '2025-10-17 00:00:00', '獨創', 0, 1, ''),
(6, 'BigBaitGold', 'BigBaitGold', 'https://content001.bet365.mx/Games/SGP/GamePodImages/BigBaitGold_1000x400.svg', 'G10171745', '2025-10-12 00:00:00', '獨創', 0, 1, ''),
(7, 'VaultScavengers', 'VaultScavengers', 'https://content001.bet365.mx/Games/SGP/GamePodImages/VaultScavengers_1000x400_With-Character.jpg', 'G10171746', '2025-10-17 00:00:00', '獨創', 0, 1, ''),
(8, 'UntamedGorillaRubyRiseV3', 'UntamedGorillaRubyRiseV3', 'https://content001.bet365.mx/Games/SGP/GamePodImages/UntamedGorillaRubyRiseV3_1000x400.svg', 'G17174955', '2025-10-07 00:00:00', '獨創', 0, 1, ''),
(9, 'CrystalStacks', 'CrystalStacks', 'https://content001.bet365.mx/Games/SGP/GamePodImages/CrystalStacks_1000x400.jpg', 'G17175029', '2025-10-17 00:00:00', '高報酬', 0, 1, ''),
(10, 'MiniSuperSpinRoulette', 'MiniSuperSpinRoulette', 'https://content001.bet365.mx/Games/SGP/GamePodImages/MiniSuperSpinRoulette_1000x400.svg', 'G17175128', '2025-10-17 00:00:00', '高報酬', 0, 1, ''),
(11, 'SamuraiVSNinja', 'SamuraiVSNinja', 'https://content001.bet365.mx/Games/SGP/GamePodImages/SamuraiVSNinja_1000x400_With-Character.jpg', 'G17175437', '2025-10-17 00:00:00', '高報酬', 0, 1, ''),
(12, 'Jesterprize5000', 'Jesterprize5000', 'https://content001.bet365.mx/Games/SGP/GamePodImages/Jesterprize5000_1000x400_With-Character.jpg', 'G17175513', '2025-10-17 00:00:00', '高報酬', 0, 1, ''),
(13, '8ImmortalsTrialsofLiTieguai', '8ImmortalsTrialsofLiTieguai', 'https://content001.bet365.mx/Games/SGP/GamePodImages/8ImmortalsTrialsofLiTieguai_1000x400_With-Character.jpg', 'G17175534', '2025-10-17 00:00:00', '高報酬', 0, 1, ''),
(14, 'EdgeOfSleepHotel2', 'EdgeOfSleepHotel2', 'https://content001.bet365.mx/Games/SGP/GamePodImages/EdgeOfSleepHotel2_1000x400.jpg', 'G17175554', '2025-10-17 00:00:00', '高報酬', 0, 1, ''),
(15, 'BuffaloKingMegaways', 'BuffaloKingMegaways', 'https://content001.bet365.mx/Games/SGP/GamePodImages/BuffaloKingMegaways_1000x400_With-Character.jpg', 'G17175615', '2025-10-17 00:00:00', '經典', 0, 1, ''),
(16, 'GatesofOlympus1000', 'GatesofOlympus1000', 'https://content001.bet365.mx/Games/SGP/GamePodImages/GatesofOlympus1000_1000x400_With-Character.jpg', 'G17175642', '2025-10-17 00:00:00', '經典', 0, 1, ''),
(17, 'RazorReturnsbet365', 'RazorReturnsbet365', 'https://content001.bet365.mx/Games/SGP/GamePodImages/RazorReturnsbet365_1000x400_With-Character.jpg', 'G17175747', '2025-10-17 00:00:00', '經典', 0, 1, ''),
(18, 'LeKing', 'LeKing', 'https://content001.bet365.mx/Games/SGP/GamePodImages/LeKing_1000x400_With-Character.jpg', 'G17175805', '2025-10-17 00:00:00', '經典', 0, 1, ''),
(19, 'WildWildRichesMegaways', 'WildWildRichesMegaways', 'https://content001.bet365.mx/Games/SGP/GamePodImages/WildWildRichesMegaways_1000x400_With-Character.jpg', 'G17175825', '2025-10-17 00:00:00', '經典', 0, 1, ''),
(20, 'TedMegaways', 'TedMegaways', 'https://content001.bet365.mx/Games/SGP/GamePodImages/TedMegaways_1000x400_With-Character.jpg', 'G22123639', '2025-10-22 00:00:00', '經典', 0, 1, ''),
(21, '10001NightsMegaways', '10001NightsMegaways', 'https://content001.bet365.mx/Games/SGP/GamePodImages/10001NightsMegaways_v2_1000x400_With-Character.jpg', 'G22123706', '2025-10-22 00:00:00', '經典', 0, 1, ''),
(22, 'EternalPhoenixMegaways', 'EternalPhoenixMegaways', 'https://content001.bet365.mx/Games/SGP/GamePodImages/EternalPhoenixMegaways_1000x400_With-Character.jpg', 'G22123811', '2025-10-22 00:00:00', '頂級', 0, 1, ''),
(23, 'BigWheelBonus', 'BigWheelBonus', 'https://content001.bet365.mx/Games/SGP/GamePodImages/BigWheelBonus_1000x400.svg', 'G22123841', '2025-10-22 00:00:00', '頂級', 0, 1, ''),
(24, 'TripleRedHot7sFreeGames', 'TripleRedHot7sFreeGames', 'https://content001.bet365.mx/Games/SGP/GamePodImages/TripleRedHot7sFreeGames_1000x400.jpg', 'G22123902', '2025-10-22 00:00:00', '頂級', 0, 1, ''),
(25, 'LostRelics', 'LostRelics', 'https://content001.bet365.mx/Games/SGP/GamePodImages/LostRelics_1000x400_With-Character.jpg', 'G22123924', '2025-10-22 00:00:00', '頂級', 0, 1, ''),
(26, 'Explosive-Frenzy', 'Explosive-Frenzy', 'https://content001.bet365.mx/Games/SGP/GamePodImages/Explosive-Frenzy_1000x400_With-Character.jpg', 'G22124039', '2025-10-22 00:00:00', '頂級', 0, 1, ''),
(27, 'MummysJewels', 'MummysJewels', 'https://content001.bet365.mx/Games/SGP/GamePodImages/MummysJewels_1000x400.svg', 'G22124056', '2025-10-22 00:00:00', '頂級', 0, 1, ''),
(28, 'AztecBonanza', 'AztecBonanza', 'https://content001.bet365.mx/Games/SGP/GamePodImages/AztecBonanza_1000x400_With-Character.jpg', 'G22124130', '2025-10-22 00:00:00', '獨創', 0, 1, '');

--
-- 已傾印資料表的索引
--

--
-- 資料表索引 `gamelist`
--
ALTER TABLE `gamelist`
  ADD PRIMARY KEY (`GameID`);

--
-- 在傾印的資料表使用自動遞增(AUTO_INCREMENT)
--

--
-- 使用資料表自動遞增(AUTO_INCREMENT) `gamelist`
--
ALTER TABLE `gamelist`
  MODIFY `GameID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
