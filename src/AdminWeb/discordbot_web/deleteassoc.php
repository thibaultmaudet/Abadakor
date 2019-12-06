<?php
include "inc/functions.php";
include "inc/db.php";
$user = $_GET['user'];
$course = $_GET['course']
//SQL
$req = $pdo->prepare('DELETE FROM assoc_coursesusers WHERE id_user = ? AND id_course = ?');
$req->execute(array($user, $course));
//
header('location : assoc.php');
exit();
?>

