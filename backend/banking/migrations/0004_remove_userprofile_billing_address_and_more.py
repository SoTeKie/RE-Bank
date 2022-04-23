# Generated by Django 4.0.2 on 2022-04-23 23:13

from django.db import migrations, models
import django.db.models.deletion


class Migration(migrations.Migration):

    dependencies = [
        ('banking', '0003_remove_userprofile_shipping_address'),
    ]

    operations = [
        migrations.RemoveField(
            model_name='userprofile',
            name='billing_address',
        ),
        migrations.AddField(
            model_name='billingaddress',
            name='user',
            field=models.OneToOneField(default='6b5ca190-add8-421e-90f3-26bab9698546', on_delete=django.db.models.deletion.CASCADE, related_name='billing_address', to='banking.userprofile'),
            preserve_default=False,
        ),
        migrations.AddField(
            model_name='shippingaddress',
            name='user',
            field=models.OneToOneField(default='6b5ca190-add8-421e-90f3-26bab9698546', on_delete=django.db.models.deletion.CASCADE, related_name='shipping_address', to='banking.userprofile'),
            preserve_default=False,
        ),
    ]
