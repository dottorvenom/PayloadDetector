
<?php



$servername = "localhost";
$username = "root";
$password = "";

$conn = new mysqli($servername, $username, $password);
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}


if( isset($_REQUEST["payload"]) && isset($_REQUEST["idlog"])) {
	
	$payload = $_REQUEST["payload"];  
	$idlog = $_REQUEST["idlog"];
	$ip = $_SERVER['REMOTE_ADDR'];
	
	
	$sql = "INSERT INTO c2.log_monitor (ip_address_source, event_full, id_log) VALUES ('" . $ip . "','" . $payload . "','" . $idlog . "')";
	
	$result = $conn->query($sql);

	if ($result) {
		echo "OK";
	} else {
		echo "ERR";
	}

}


 
?>

