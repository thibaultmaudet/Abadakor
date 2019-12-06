<?php
//include PHPMailerAutoload.php
require '/PHPMailerAutoload.php';

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
$mail->Subject = "test email";

//set HTML to true
$mail->isHTML(true);

//set body
$mail->Body = "MisterAntonin<br /><br /><a href='http://google.com'>Google</a> ";

//set who is sending an email
$mail->setFrom('testantoninrichard@gmail.com', 'Antonin RICHARD');

//set where we are sending email (recipients)
$mail->addAddress('mister.antonin@gmail.com');

//send an email
if ($mail->send())
    echo "mail is sent";
else
    echo $mail->ErrorInfo;
?>
