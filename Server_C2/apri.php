
<?php


function parsingBase64($m) {
  $i = strpos($m, 'Code:')+6;
  return trim(substr($m,$i));  
}


$servername = "localhost";
$username = "root";
$password = "";

$conn = new mysqli($servername, $username, $password);
if ($conn->connect_error) {
	die;
  
}

$id = $_REQUEST["id"]; 
$sql = "SELECT * FROM c2.log_monitor where id = '" . $id . "'";

$result = $conn->query($sql);


if ($result->num_rows > 0) {
	
	$row = $result->fetch_assoc();
    $b = parsingBase64($row["event_full"]);
	
    $data = base64_decode($b);

    header("Cache-Control: no-cache private");
    header("Content-Description: File Transfer");
    header("Content-disposition: attachment; filename=download_" . $id . ".dat");      
    header("Content-Type: application/octet-stream");
    header("Content-Transfer-Encoding: binary");
    header('Content-Length: '. strlen($data));      
    header("Pragma: no-cache");
    header("Expires: 0");
	
    echo $data;
	

} 

 
?>
