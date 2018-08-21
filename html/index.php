<?php

require 'vendor/autoload.php';

$app = new \Slim\Slim();

$app->get('/getbalance/:account', function ($account) {
    $output = shell_exec('sudo bash /home/naokiishida/sovolcoin/API/getbalance.sh '.$account);
    echo $output;
});

$app->get('/getinfo', function () {
    $output = shell_exec('sudo bash /home/naokiishida/sovolcoin/API/utils.sh getinfo');
    echo "<pre>$output</pre>";
});

$app->run();
