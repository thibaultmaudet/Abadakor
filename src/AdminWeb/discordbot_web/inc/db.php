<?php
$pdo = new PDO('mysql:dbname=w8ix08_abadakor;host=sql.antoninrichard.fr', 'w8ix08_abadakor', 'antoninrichard',array(PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8'));
$pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
$pdo->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_OBJ);
