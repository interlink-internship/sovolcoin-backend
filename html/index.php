<?php
  
require 'vendor/autoload.php';

$app = new \Slim\Slim();

$app->get('/api/getbalance/:account', function ($account) {
    header("Access-Control-Allow-Origin: *");
    $accountlist_json = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/listaccounts.sh');
    $accountlist = array_keys(json_decode($accountlist_json,true));
    if(!in_array($account,$accountlist)){
        $array = ['state' => 'ng','error' => 'no such account'];
        echo json_encode($array);
    }
    else{
        $output = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getbalance.sh '.$account);
        $output = intval($output);
        $array = ['state' => 'ok','balance' => $output];
        echo json_encode($array);
    }
});
$app->get('/api/createaccount/:account', function ($account) {
    header("Access-Control-Allow-Origin: *");
    $accountlist_json = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/listaccounts.sh');
    $accountlist = array_keys(json_decode($accountlist_json,true));
    if(in_array($account,$accountlist)){
        $array = ['state' => 'ng','error' => 'used name'];
        echo json_encode($array);
    }
    else{   
        $address = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/createaccount.sh '.$account);
        $privatekey = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getprivatekey.sh '.$address);
        $array = ['state' => 'ok','address' => substr($address,0,strlen($address)-1),'privatekey' => substr($privatekey,0,strlen($privatekey)-1)];
        echo json_encode($array);
    }
});
$app->get('/api/sendmoneytoaddress/:account/:privatekey/:num/:To', function ($account,$privatekey,$num,$To) {
    header("Access-Control-Allow-Origin: *");
    $accountlist_json = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/listaccounts.sh');
    $accountlist = array_keys(json_decode($accountlist_json,true));
    if(!in_array($account,$accountlist)){
        $array = ['state' => 'ng','error' => 'no such account'];
        echo json_encode($array);
    }
    else{   
        $address_list = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getaccountaddress.sh '.$account);
        $address_list = json_decode($address_list);
        $ok = FALSE;
        foreach($address_list as $address){
            $key = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getprivatekey.sh '.$address);
            $key = substr($key,0,strlen($key)-1);
            $ok = $ok || (strcmp($privatekey,$key) == 0);
        }
        if(!$ok){
            $array = ['state' => 'ng','error' => 'incorrect key'];
            echo json_encode($array);
        }
        else{
            $output = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/send.sh '.$account.' '.$To.' '.$num.' 2>&1');
            if(strcmp("error: ",substr($output,0,7)) == 0){
                $array = ['state' => 'ng','error' => 'num is wrong'];
                echo json_encode($array);
            }
            else if(strcmp("error code: ",substr($output,0,12)) == 0){
                if($output[13] == '6'){
                    $array = ['state' => 'ng','error' => 'account has insufficient funds'];
                    echo json_encode($array);
                }
                else{
                    $array = ['state' => 'ng','error' => 'invalid address'];
                    echo json_encode($array);
                }
            }
            else{
                $output = substr($output,0,strlen($output)-1);
                $array = ['state' => 'ok','transaction' => $output];
                echo json_encode($array);
            }
        }
    }
});
$app->get('/api/sendmoneytoaccount/:account/:privatekey/:num/:To', function ($account,$privatekey,$num,$To) {
    header("Access-Control-Allow-Origin: *");
    $accountlist_json = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/listaccounts.sh');
    $accountlist = array_keys(json_decode($accountlist_json,true));
    if(!in_array($account,$accountlist) || !in_array($To,$accountlist)){
        $array = ['state' => 'ng','error' => 'no such account'];
        echo json_encode($array);
    }
    else{   
        $address_list = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getaccountaddress.sh '.$account);
        $address_list = json_decode($address_list);
        $ok = FALSE;
        foreach($address_list as $address){
            $key = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getprivatekey.sh '.$address);
            $key = substr($key,0,strlen($key)-1);
            $ok = $ok || (strcmp($privatekey,$key) == 0);
        }
        if(!$ok){
            $array = ['state' => 'ng','error' => 'incorrect key'];
            echo json_encode($array);
        }
        else{
            $to_address_list = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/getaccountaddress.sh '.$To);
            $to_address_list = json_decode($to_address_list);
            $to_address = $to_address_list[0];
            $output = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/send.sh '.$account.' '.$to_address.' '.$num.' 2>&1');
            if(strcmp("error: ",substr($output,0,7)) == 0){
                $array = ['state' => 'ng','error' => 'num is wrong'];
                echo json_encode($array);
            }
            else if(strcmp("error code: ",substr($output,0,12)) == 0){
                if($output[13] == '6'){
                    $array = ['state' => 'ng','error' => 'account has insufficient funds'];
                    echo json_encode($array);
                }
                else{
                    $array = ['state' => 'ng','error' => 'invalid address'];
                    echo json_encode($array);
                }
            }
            else{
                $output = substr($output,0,strlen($output)-1);
                $array = ['state' => 'ok','transaction' => $output];
                echo json_encode($array);
            }
        }
    }
});
$app->get('/api/getinfo', function () {
    header("Access-Control-Allow-Origin: *");
    $output = shell_exec('sudo bash /home/sovolcoin/AltcoinGenerator/API/utils.sh getinfo');
    echo "<pre>$output</pre>";
});
$app->get('/', function () use ($app) {
    $app->response->redirect('/build/index.html', 303);
});
$app->run();
