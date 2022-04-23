# Generated by Django 4.0.2 on 2022-04-23 16:54

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ("banking", "0002_userprofile_middle_name_userprofile_title"),
    ]

    operations = [
        migrations.AlterField(
            model_name="bankaccount",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="billingaddress",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="card",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="childparent",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="shippingaddress",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="transaction",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="userprofile",
            name="billing_address",
            field=models.OneToOneField(
                blank=True,
                null=True,
                on_delete=django.db.models.deletion.CASCADE,
                to="banking.billingaddress",
            ),
        ),
        migrations.AlterField(
            model_name="userprofile",
            name="deleted_at",
            field=models.DateTimeField(blank=True, null=True),
        ),
        migrations.AlterField(
            model_name="userprofile",
            name="shipping_address",
            field=models.OneToOneField(
                blank=True,
                null=True,
                on_delete=django.db.models.deletion.CASCADE,
                to="banking.shippingaddress",
            ),
        ),
    ]