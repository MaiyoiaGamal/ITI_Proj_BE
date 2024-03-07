## ITI project 
Run these command to migrate db

1- Open Visual Studio.

2- In the top menu, go to View -> Other Windows -> Package Manager Console to open the Package Manager Console.

3- Run the following command: 
sh ``` 
Add-Migration MAI
Update-Database```

### Import Note: Issue with db migration so Updated
line: https://github.com/MaiyoiaGamal/ITI_Proj_BE/blob/main/proj2/Models/HRContext.cs#L20
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NetSalary>()
            .Property(ns => ns.netsalary)
            .HasColumnType("decimal(18,2)");
    }
