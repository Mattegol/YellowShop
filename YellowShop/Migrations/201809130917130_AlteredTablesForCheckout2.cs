namespace YellowShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlteredTablesForCheckout2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            AlterColumn("dbo.OrderItems", "OrderId", c => c.Int());
            CreateIndex("dbo.OrderItems", "OrderId");
            AddForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            AlterColumn("dbo.OrderItems", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderItems", "OrderId");
            AddForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
        }
    }
}
