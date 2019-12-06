<?php
require_once 'inc/functions.php';
session_start();
if(!empty($_POST)){

    $errors = array();
    require_once 'inc/db.php';

    if(empty($_POST['username']) || !preg_match('/^[a-zA-Z0-9_]+$/', $_POST['username'])){
        $errors['username'] = "Votre pseudo n'est pas valide (alphanumérique)";
    } else {
        $req = $pdo->prepare('SELECT id FROM account WHERE username = ?');
        $req->execute([$_POST['username']]);
        $user = $req->fetch();
        if($user){
            $errors['username'] = 'Ce pseudo est déjà pris';
        }
    }

    if(empty($_POST['email']) || !filter_var($_POST['email'], FILTER_VALIDATE_EMAIL)){
        $errors['email'] = "Votre email n'est pas valide";
    } else {
        $req = $pdo->prepare('SELECT id FROM account WHERE email = ?');
        $req->execute([$_POST['email']]);
        $user = $req->fetch();
        if($user){
            $errors['email'] = 'Cet email est déjà utilisé pour un autre compte';
        }
    }

    if(empty($_POST['password']) || $_POST['password'] != $_POST['password_confirm']){
        $errors['password'] = "Vous devez rentrer un mot de passe valide";
    }

if(empty($errors)){

    $req = $pdo->prepare("INSERT INTO account SET username = ?, password = ?, email = ?, confirmation_token = ?, nom = ?, prenom = ?");
    $password = password_hash($_POST['password'], PASSWORD_BCRYPT);
    $token = str_random(60);
    $req->execute([$_POST['username'], $password, $_POST['email'], $token, $_POST['nom'],$_POST['prenom']]);
    $user_id = $pdo->lastInsertId();

    //MAIL
    //mail($_POST['email'], 'Confirmation de votre compte', "Afin de valider votre compte merci de cliquer sur ce lien\n\n");

    //include PHPMailerAutoload.php
    require 'asset/phpmailer/PHPMailerAutoload.php';

    //create an instance of PHPMailer
    $mail = new PHPMailer();

    //set a host
    $mail->Host = "smtp.gmail.com";

    //enable SMTP
    $mail->isSMTP();
    $mail->SMTPDebug = 2;

    //set authentication to true
    $mail->SMTPAuth = true;

    //set login details for Gmail account
    $mail->Username = "testantoninrichard@gmail.com";
    $mail->Password = "MotDePasse37";

    //set type of protection
    $mail->SMTPSecure = "ssl"; //or we can use TLS

    //set a port
    $mail->Port = 465; //or 587 if TLS

    //set subject
    $mail->Subject = "Confirmation d'inscription";

    //set HTML to true
    $mail->isHTML(true);

    //set body
    $usernameclair = $_POST['username'];
    $passwordclair = $_POST['password'];
    $mail->Body = "Merci de votre inscription<br /><br /><a href='http://www.antoninrichard.fr/discordbot_web/confirm.php?id=$user_id&token=$token'>Cliquez sur le lien pour valider le compte</a><br /><br/><p>Vos identifiants sont : <br>Nom d'utilisateur = $usernameclair <br>Mot de passe = $passwordclair <br></p> ";

//set who is sending an email
    $mail->setFrom('no-reply@antoninrichard.fr', 'ABADAKOR BOT DISCORD');

//set where we are sending email (recipients)
    $mail->addAddress($_POST['email']);

//send an email
    if ($mail->send())
        echo "mail is sent";
    else
        echo $mail->ErrorInfo;

    //MAIL
    $_SESSION['flash']['success'] = 'Un email de confirmation vous a été envoyé pour valider votre compte';
    header('Location: login.php');
    exit();
}


}
?>

<?php require 'inc/header.php'; ?>

<h1>S'inscrire</h1>

<?php if(!empty($errors)): ?>
<div class="alert alert-danger">
    <p>Vous n'avez pas rempli le formulaire correctement</p>
    <ul>
        <?php foreach($errors as $error): ?>
           <li><?= $error; ?></li>
        <?php endforeach; ?>
    </ul>
</div>
<?php endif; ?>

<form action="" method="POST">

    <div class="form-group">
        <label for="">username</label>
        <input type="text" name="username" class="form-control"/>
    </div>
    <div class="form-group">
        <label for="">Nom</label>
        <input type="text" name="nom" class="form-control"/>
    </div>
    <div class="form-group">
        <label for="">Prenom</label>
        <input type="text" name="prenom" class="form-control"/>
    </div>
    <div class="form-group">
        <label for="">Email</label>
        <input type="text" name="email" class="form-control"/>
    </div>

    <div class="form-group">
        <label for="">Mot de passe</label>
        <input type="password" name="password" class="form-control"/>
    </div>

    <div class="form-group">
        <label for="">Confirmez votre mot de passe</label>
        <input type="password" name="password_confirm" class="form-control"/>
    </div>

    <button type="submit" class="btn btn-primary">M'inscrire</button>

</form>

<?php require 'inc/footer.php'; ?>
