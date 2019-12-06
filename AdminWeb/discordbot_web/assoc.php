<?php include'inc/header.php'; ?>
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.1/css/font-awesome.min.css">
<div class="container-fluid">
        <p>
            <h1 class="page-header" style="text-align: center">EDF BUSINESS FORM: PANEL</h1>

            <h2 class="sub-header" style="margin-top: 50px; margin-bottom: 0px">Mes clients:</h2>

    <p style="text-align: right; margin-bottom: 20px"><a class="btn btn-primary" href="import.php" role="button">Importer</a>
                <a class="btn btn-primary" href="ajoutclient.php" role="button">Ajouter une fiche</a></p>
    <div class="table-responsive">
                <table id="table-panel" class="table table-striped" style="font-size: 16px">
                    <thead>
                    <tr>
                        <th data-sort="int">#ID</th>
                        <th data-sort="string">Nom</th>
                        <th>Secteur d'activité</th>
                        <th data-sort="int">Code Postal</th>
                        <th data-sort="string">Ville</i> </th>
                        <th>Score moyen</th>
                        <th style="text-align: right">Action</th>
                    </tr>
                    </thead>
                    <tbody>
                    <?php
                    include 'inc/db.php';
                    include 'inc/functions.php';
                    // REQUETE SQL
                    $user_id = $_SESSION['auth']->id;
                    $req = $pdo->prepare('SELECT * FROM `client` WHERE id_createur_client = ? OR  commercial_client = ? OR manager_client = ? OR expert_client = ?');
                    $req->execute(array($user_id, $user_id, $user_id, $user_id));
                    //
                    while ($data = $req->fetch(PDO::FETCH_ASSOC))
                    {
                        echo '
                        <tr>
                        <td>'. $data['id_client'] .'</td>
                        <td>'. $data['nom_client'] .'</td>
                        <td>'. $data['secteur_client'] .'</td>
                        <td>' . $data['codepostal_client'] .'</td>
                        <td>'. $data['ville_client'] .'</td>
                        <td> 0 </td>
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
