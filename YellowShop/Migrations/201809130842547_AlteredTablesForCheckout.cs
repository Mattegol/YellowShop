namespace YellowShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteredTablesForCheckout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "BillingAddress", c => c.String());
            AddColumn("dbo.Customers", "BillingCity", c => c.String());
            AddColumn("dbo.Customers", "BillingState", c => c.String());
            AddColumn("dbo.Customers", "BillingPostalCode", c => c.String());
            AddColumn("dbo.Orders", "ShippingAddress", c => c.String());
            AddColumn("dbo.Orders", "ShippingCity", c => c.String());
            AddColumn("dbo.Orders", "ShippingState", c => c.String());
            AddColumn("dbo.Orders", "ShippingPostalCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ShippingPostalCode");
            DropColumn("dbo.Orders", "ShippingState");
            DropColumn("dbo.Orders", "ShippingCity");
            DropColumn("dbo.Orders", "ShippingAddress");
            DropColumn("dbo.Customers", "BillingPostalCode");
            DropColumn("dbo.Customers", "BillingState");
            DropColumn("dbo.Customers", "BillingCity");
            DropColumn("dbo.Customers", "BillingAddress");
        }
    }
}
