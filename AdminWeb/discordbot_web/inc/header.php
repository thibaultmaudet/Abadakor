<?php
if(session_status() == PHP_SESSION_NONE){
    session_start();
}
mb_internal_encoding('UTF-8');
?><!DOCTYPE html>
<html lang="fr">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <meta name="description" content="">
    <meta name="author" content="">
    <title>ABADAKOR BOT</title>

    <!-- Bootstrap core CSS -->
    <script src="asset/js/jquery.js"></script>
    <link href="asset/css/app.css" rel="stylesheet">
    <script src="asset/css/bootstrap.js"></script>
    <script type="application/javascript">
        function dropbutton(x){
            x.getElementById(x).style.backgroundColor="#246aba";
            x.getElementById(x).style.borderColor="#070da1";
        }
    </script>
</head>

<body>

<nav class="navbar navbar-inverse">
    <div class="container" style="margin-top: 10px; margin-bottom: 10px">
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a href="index.php" class="navbar-left"><img style="width: 50%; height: 50%" src="asset/img/abadakor.png"></a>
        </div>
        <div id="navbar" class="collapse navbar-collapse">
            <ul class="nav navbar-nav" style="margin-left: 20px; font-size: medium">
                <?php if (isset($_SESSION['auth'])): ?>
                    <li><a href="account.php">Mon compte</a></li>
                    <li><a href="users.php">Mes utilisateurs</a></li>
                    <li><a href="courses.php">Mes cours</a></li>
                    <li><a href="logout.php">Se déconnecter</a></li>
                <?php else: ?>
                    <li><a href="login.php">Se connecter</a></li>
                    <li><a href="register.php">S'inscrire</a></li>
                <?php endif; ?>
            </ul>
        </div>
    </div>
</nav>

<div class="container">

    <?php if(isset($_SESSION['flash'])): ?>
        <?php foreach($_SESSION['flash'] as $type => $message): ?>
            <div class="alert alert-<?= $type; ?>">
                <?= $message; ?>
            </div>
        <?php endforeach; ?>
        <?php unset($_SESSION['flash']); ?>
    <?php endif; ?>

