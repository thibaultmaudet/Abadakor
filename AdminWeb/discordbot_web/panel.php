<?php include'inc/header.php'; ?>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.1/css/font-awesome.min.css">
<div class="container-fluid">
        <p>
            <h1 class="page-header" style="text-align: center">Mes utilisateurs:</h1>

    <div class="table-responsive">
                <table id="table-panel" class="table table-striped" style="font-size: 16px">
                    <thead>
                    <tr>
                        <th>#ID</th>
                        <th>Nom</th>
                        <th>Prénom</th>
                        <th>Action</th>
                    </tr>
                    </thead>
                    <tbody>
                    <?php
                    include 'inc/db.php';
                    include 'inc/functions.php';
                    // REQUETE SQL
                    $user_id = $_SESSION['auth']->id;
                    $req = $pdo->prepare('SELECT * FROM `users`');
                    $req->execute();
                    //
                    while ($data = $req->fetch(PDO::FETCH_ASSOC))
                    {
                        echo '
                        <tr>
                        <td>'. $data['id'] .'</td>
                        <td>'. $data['nom'] .'</td>
                        <td>'. $data['prenom'] .'</td>
                        <td style="text-align: right"><a style="color: #007196" href="question.php?id=' . $data['id_client'] .'">Modifier</a> ou  <a style="color: red" href="deleteclient.php?id='.$data['id_client'].'" onclick="return confirm(\'Etes-vous sûr ?\');">Supprimer</a></td>
                        </tr>';
                    }
                    $req->closeCursor();
                    ?>

                    </tbody>
                </table>
                <script src="https://code.jquery.com/jquery-2.2.3.min.js"></script>
                <script src="asset/js/stupidtable.min.js"></script>
                <script>
                    $(document).ready(function($) {
                        $("table-panel").stupidtable();
                    });
                </script>
            </div>
        </div>
    </div>
</div>
<?php include'inc/footer.php'; ?>
