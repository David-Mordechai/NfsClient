sudo apt update
sudo apt install nfs-kernel-server rpcbind
sudo mkdir /nfsshare
sudo chown nobody:nogroup /nfsshare
sudo chmod 777 /nfshare
sudo sh -c "echo '/nfsshare *(rw,sync,no_subtree_check,insecure)' >> /etc/exports"
sudo cat /etc/exports
sudo service rpcbind start
ps aux
sudo service nfs-kernel-server start


ssh davidm@172.24.131.49

touch log.csv
nano log.csv
cat log.csv