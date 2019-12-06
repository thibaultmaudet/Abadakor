<?php
include "inc/functions.php";
include "inc/db.php";
$id = $_GET['id'];
//SQL
$req = $pdo->prepare('DELETE FROM Courses WHERE id = ?');
$req->execute(array($id));
//
header('location : courses.php');
exit();
?>

