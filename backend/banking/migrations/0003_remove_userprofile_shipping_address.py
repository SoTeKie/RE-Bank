# Generated by Django 4.0.2 on 2022-04-23 23:10

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('banking', '0002_billingaddress_city_shippingaddress_city_and_more'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='userprofile',
            name='shipping_address',
        ),
    ]
