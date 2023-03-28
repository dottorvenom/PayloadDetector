<?php

include 'conf.php';
include 'db.php';

?>

<html>

<head>
<!-- <meta http-equiv="refresh" content="60"> -->

<!--
<link rel="stylesheet" href="./css/collapse.css">
<script src="./js/collapse.js"></script>
-->

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
				location.href="process.php?do=D&id=" + i;  
			}   
		} 
	
</script>


<br>


<?php


		if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="D" ) {
			
			$id = $_REQUEST["id"];
			$sql = "DELETE from c2.tbl_ips where id=" . $id;
			$result = $conn->query($sql);
			
			$sql = "DELETE from c2.tbl_ips_process where id_parent_ip=" . $id;
			$result = $conn->query($sql);
			
			
			//eliminare le dll con gli id processi
			$sql = "DELETE from c2.tbl_ips_process_dll where id_parent_ip=" . $id;
			$result = $conn->query($sql);
			
			
		}
		
		
	?>



<div class="container-fluid">

<table class="table table-bordered table-hover">

<thead>
<tr>
	<td colspan="3" align="center"><h3>IP / Process / DLL</h3></td>
</tr>
</thead>

<tbody>
<tr>
	<td colspan="3">&nbsp;</td> 
</tr>

<tr>
	<td colspan="3"><a href="index.php">Home</a>&nbsp;|&nbsp;
	<?php
		if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="H" ) {
			echo '<a href="process.php">Show all</a>';
		}else{
			echo '<a href="process.php?do=H">Hide Windows fold</a>';
		}
	?>
	&nbsp;|&nbsp;<a href="process.php">Update</a></td>
</tr>

	  
<?php


$sql = "SELECT id,ip_address,hostname from c2.tbl_ips order by id ASC";
$result = $conn->query($sql);


if ($result->num_rows > 0) {
  while($row = $result->fetch_assoc()) {	
  
	$ip =  $row["ip_address"];
	$hostname = $row["hostname"];
	$id = $row["id"];
	
	
	echo "<tr>";
	
	echo "<td width='400px'>";
	echo "IP: " . $ip . " Hostname: " . $hostname;
	echo "</td>";
	echo "<td>";
	
	echo '<div class="accordion" id="accordionExample">';
	
	
	$sqlp = "SELECT id,id_parent_ip,process_name from c2.tbl_ips_process where id_parent_ip = " . $id . " order by process_name ASC";  //" GROUP BY process_name" 
	$resultp = $conn->query($sqlp);
	if ($resultp->num_rows > 0) {
		
		
		
		while($rowp = $resultp->fetch_assoc()) {
			
			$idp = $rowp["id"];
			echo '<div class="card">';
			
			
			echo '<div class="card-header" id="heading' . $idp . '">';
			
			echo '<button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapse' . $idp . '" aria-expanded="true" aria-controls="collapse' . $idp . '">';
			echo $rowp["process_name"];
			echo '</button>';

			echo '</div>';
			
			if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="H" ) {
				$sqld = "SELECT id,id_parent_proc,id_parent_ip,hash,dll_path from c2.tbl_ips_process_dll where id_parent_proc = " . $idp . " and dll_path NOT LIKE '%windows%' order by dll_path ASC";  //" GROUP BY hash" 
			}else{
				$sqld = "SELECT id,id_parent_proc,id_parent_ip,hash,dll_path from c2.tbl_ips_process_dll where id_parent_proc = " . $idp . " order by dll_path ASC";  //" GROUP BY hash" 
			}
			
			$resultd = $conn->query($sqld);
			
			
			if ($resultd->num_rows > 0) {
				
				echo '<div id="collapse' . $idp . '" class="collapse" aria-labelledby="heading' . $idp . '" data-parent="#accordionExample">';
				echo '<div class="card-body">';
				
				echo "<ul>";
				while($rowd = $resultd->fetch_assoc()) {
					echo "<li>" . $rowd["dll_path"] . " " . "<a target='_blank' href='https://www.virustotal.com/gui/search/" . $rowd["hash"] . "'>" . $rowd["hash"] . '</a></li>';
				}
				echo "</ul>";
				
				echo '</div>';
				echo '</div>';
			}
			
			echo '</div>';
		}
		
		

	
	}
	

  }
  
  echo '</div>';
	
	echo "</td>";
	echo "<td><a href=\"javascript:myconf(" . $id . ");\">Delete</a></td>";
	echo "</tr>";
	
	
} 



 
?>





<?php
if ($result->num_rows > 0) {

	echo '<tr>';
	echo '<td colspan="3"><a href="index.php">Home</a>&nbsp;|&nbsp;';
	
		if( isset($_REQUEST["do"]) && $_REQUEST["do"]=="H" ) {
			echo '<a href="process.php">Show all</a>';
		}else{
			echo '<a href="process.php?do=H">Hide Windows fold</a>';
		}
	
	echo '&nbsp;|&nbsp;<a href="process.php">Update</a></td>';
	echo '</tr>';
}else {
	//echo '<tr>';
	//echo '<td colspan="3">&nbsp;</td>';
	//echo '</tr>';
}
?>

</tbody>
</table>

	
	
</div>
</body>
</html>