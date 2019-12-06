<?php
require_once 'inc/functions.php';
require_once 'inc/db.php';
reconnect_from_cookie();
require 'inc/header.php';
$mode = $_GET["mode"];
switch($mode){
    case "edit":
        $itemid = $_GET["id"];
        $req = $pdo->prepare('SELECT * FROM `Courses` WHERE id = ?');
        $req->execute(array($itemid));
        $data = $req->fetch(PDO::FETCH_ASSOC);
        echo '
        <h1>Cours :</h1>

        <form action="" method="POST">

        <div class="form-group">
            <label for="">ID</label>
            <input type="text" name="id" class="form-control" value="'.$data["id"].'" readonly/>
        </div>

        <div class="form-group">
            <label for="">Libelle</label>
            <input type="text" name="caption" class="form-control" value="'.$data["caption"].'" />
        </div>

        <button type="submit" class="btn btn-primary">Enregister</button>

        </form>
        ';
        $req->closeCursor();
        if(!empty($_POST)){
            $req2 = $pdo->prepare("UPDATE `Courses` SET caption = ? WHERE id = ?");
            $req2->execute(array($_POST["caption"],$itemid));
            $req2->closeCursor();
            header('Location: courses.php');
        }
    break;

    case "add":
        echo '
        <h1>Cours :</h1>

        <form action="" method="POST">

        <div class="form-group">
            <label for="">Libelle</label>
            <input type="text" name="caption" class="form-control" />
        </div>

        <button type="submit" class="btn btn-primary">Enregister</button>

        </form>
        ';

        if(!empty($_POST)){
            $req = $pdo->prepare('INSERT INTO `Courses` (`id`, `caption`) VALUES (NULL, ?);');
            $req->execute(array($_POST["caption"]));
            header('Location: courses.php');
        }
    break;

    default:
    break;
}
require 'inc/footer.php'; 
?>