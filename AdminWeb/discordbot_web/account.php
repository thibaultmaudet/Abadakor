<?php
require 'inc/functions.php';
require 'inc/db.php';
logged_only();
if(!empty($_POST)){

    if(empty($_POST['password']) || $_POST['password'] != $_POST['password_confirm']){
        $_SESSION['flash']['danger'] = "Les mots de passes ne correspondent pas";
    }else{
        $user_id = $_SESSION['auth']->id;
        $password= password_hash($_POST['password'], PASSWORD_BCRYPT);
        require_once 'inc/db.php';
        $pdo->prepare('UPDATE account SET password = ? WHERE id = ?')->execute([$password, $user_id]);
        $_SESSION['flash']['success'] = "Votre mot de passe a bien été mis à jour";
    }

}
require 'inc/header.php';
?>
    <h1>Bonjour <?= $_SESSION['auth']->prenom; ?></h1><br><br>
    <form action="" method="post">
        <h3><u>Mes informations personnelles:</u></h3>

        <div class="form-group">
            <p>Nom : <?= $_SESSION['auth']->nom ?></p>
        </div>

        <div class="form-group">
            <p>Prenom : <?= $_SESSION['auth']->prenom ?></p>
        </div>

        <div class="form-group">
            <p>Email : <?= $_SESSION['auth']->email ?></p>
        </div>

        <div class="form-group">
            <input class="form-control" type="password" name="password" placeholder="Changer de mot de passe"/>
        </div>

        <div class="form-group">
            <input class="form-control" type="password" name="password_confirm" placeholder="Confirmation du mot de passe"/>
        </div>

        <button class="btn btn-primary">Changer mon mot de passe</button>

    </form>


<?php require 'inc/footer.php'; ?>